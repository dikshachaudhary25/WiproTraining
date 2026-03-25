import { Component, OnInit, ChangeDetectorRef, Inject, PLATFORM_ID, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MessageService, Message } from '../../services/message';
import { AuthService } from '../../services/auth';
import { SignalRService } from '../../services/signalr.service';

export interface ConversationSummary {
  propertyId: number;
  propertyTitle: string;
  otherUserId: number;
  otherUserName: string;
  lastMessageText: string;
  lastMessageTime: Date;
  isUnread: boolean;
}

@Component({
  selector: 'app-messages',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './messages.html',
  styleUrls: ['./messages.css']
})
export class MessagesComponent implements OnInit, AfterViewChecked {

  conversations: ConversationSummary[] = [];
  activeConversation: ConversationSummary | null = null;
  activeThread: any[] = []; 
  
  isLoadingConversations = true;
  isLoadingThread = false;
  isSending = false;
  
  newMessageText = '';

  @ViewChild('chatHistory') private chatHistoryContainer!: ElementRef;

  constructor(
    private messageService: MessageService,
    public auth: AuthService,
    private signalRService: SignalRService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  ngOnInit() {
    if (!isPlatformBrowser(this.platformId)) {
      this.isLoadingConversations = false;
      return;
    }
    if (!this.auth.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }
    this.loadConversations();

    
    const token = this.auth.getToken();
    if (token) {
      this.signalRService.startConnection(token);
      this.signalRService.onMessageReceived((senderId: string, messageDto: any) => {
        
        if (this.activeConversation && 
           (messageDto.propertyId === this.activeConversation.propertyId) &&
           (messageDto.senderId === this.activeConversation.otherUserId || messageDto.receiverId === this.activeConversation.otherUserId)) {
           
           
           if (messageDto.senderId !== this.auth.getUserId()) {
             this.activeThread.push(messageDto);
             this.scrollToBottom();
           }
        }
        
        this.loadConversations();
      });
    }
  }

  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  private scrollToBottom(): void {
    if (this.chatHistoryContainer) {
      try {
        this.chatHistoryContainer.nativeElement.scrollTop = this.chatHistoryContainer.nativeElement.scrollHeight;
      } catch (err) {}
    }
  }

  loadConversations() {
    this.isLoadingConversations = true;
    this.messageService.getMyMessages().subscribe({
      next: (data) => {
        
        const map = new Map<string, ConversationSummary>();
        const myUserId = this.auth.getUserId();

        data.forEach(m => {
          const otherUserId = m.senderId === myUserId ? m.receiverId : m.senderId;
          const otherUserName = m.senderId === myUserId ? m.receiverName : m.senderName;
          
          const key = `${m.propertyId}_${otherUserId}`;
          const currentSentAt = new Date(m.sentAt);

          if (!map.has(key)) {
            map.set(key, {
              propertyId: m.propertyId,
              propertyTitle: m.propertyTitle || `Property #${m.propertyId}`,
              otherUserId: otherUserId,
              otherUserName: otherUserName || 'Unknown User',
              lastMessageText: m.messageText,
              lastMessageTime: currentSentAt,
              isUnread: m.receiverId === myUserId && !m.isRead
            });
          } else {
            const existing = map.get(key)!;
            
            if (currentSentAt > existing.lastMessageTime) {
              existing.lastMessageText = m.messageText;
              existing.lastMessageTime = currentSentAt;
              existing.isUnread = existing.isUnread || (m.receiverId === myUserId && !m.isRead);
            }
          }
        });

        this.conversations = Array.from(map.values()).sort((a, b) => 
          b.lastMessageTime.getTime() - a.lastMessageTime.getTime()
        );
        
        this.isLoadingConversations = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Failed to load conversations:', err);
        this.isLoadingConversations = false;
        this.cdr.detectChanges();
      }
    });
  }

  selectConversation(conv: ConversationSummary) {
    this.activeConversation = conv;
    this.isLoadingThread = true;
    this.activeThread = [];
    this.newMessageText = '';
    
    
    this.cdr.detectChanges();

    this.messageService.getConversation(conv.propertyId, conv.otherUserId).subscribe({
      next: (messages) => {
        this.activeThread = messages;
        this.isLoadingThread = false;
        this.cdr.detectChanges();
        this.scrollToBottom();
      },
      error: (err) => {
        console.error('Failed to load thread:', err);
        this.isLoadingThread = false;
        this.cdr.detectChanges();
      }
    });
  }

  backToList() {
    this.activeConversation = null;
    this.activeThread = [];
    
    this.loadConversations();
  }

  sendMessage() {
    if (!this.newMessageText.trim() || !this.activeConversation) return;

    this.isSending = true;
    const textToSend = this.newMessageText.trim();
    
    this.messageService.sendMessage({
      propertyId: this.activeConversation.propertyId,
      messageText: textToSend,
      receiverId: this.activeConversation.otherUserId
    }).subscribe({
      next: () => {
        
        this.activeThread.push({
          messageId: Date.now(), 
          senderId: this.auth.getUserId(),
          receiverId: this.activeConversation!.otherUserId,
          messageText: textToSend,
          sentAt: new Date().toISOString()
        });
        
        
        this.activeConversation!.lastMessageText = textToSend;
        this.activeConversation!.lastMessageTime = new Date();

        this.newMessageText = '';
        this.isSending = false;
        this.cdr.detectChanges();
        this.scrollToBottom();
      },
      error: (err) => {
        console.error('Failed to send message:', err);
        this.isSending = false;
        alert('Failed to send message. Please try again.');
        this.cdr.detectChanges();
      }
    });
  }
}
