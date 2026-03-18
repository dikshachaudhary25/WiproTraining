interface Contact {
    id: number;
    name: string;
    email: string;
    phone: string;
}

class ContactManager {
    private contacts: Contact[] = [];

    // Add Contact
    addContact(contact: Contact): void {
        const exists = this.contacts.find(c => c.id === contact.id);

        if (exists) {
            console.log("Contact with this ID already exists.");
            return;
        }

        this.contacts.push(contact);
        console.log("Contact added successfully.");
    }

    // View Contacts
    viewContacts(): Contact[] {
        if (this.contacts.length === 0) {
            console.log("No contacts found.");
        }
        return this.contacts;
    }

    // Modify Contact
    modifyContact(id: number, updatedContact: Partial<Contact>): void {
        const contact = this.contacts.find(c => c.id === id);

        if (!contact) {
            console.log("Contact not found.");
            return;
        }

        Object.assign(contact, updatedContact);
        console.log("Contact updated successfully.");
    }

    // Delete Contact
    deleteContact(id: number): void {
        const index = this.contacts.findIndex(c => c.id === id);

        if (index === -1) {
            console.log("Contact not found.");
            return;
        }

        this.contacts.splice(index, 1);
        console.log("Contact deleted successfully.");
    }
}
// TESTING

const manager = new ContactManager();

// Add Contacts
manager.addContact({ id: 1, name: "Diksha", email: "diksha@gmail.com", phone: "1234567890" });
manager.addContact({ id: 2, name: "Rahul", email: "rahul@gmail.com", phone: "9876543210" });

// View Contacts
console.log("Contacts:", manager.viewContacts());

// Modify Contact
manager.modifyContact(1, { phone: "1111111111" });

// Delete Contact
manager.deleteContact(2);

// Try deleting non-existing contact
manager.deleteContact(5);

// Final List
console.log("Final Contacts:", manager.viewContacts());