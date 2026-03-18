import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-category-bar',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './category-bar.html',
    styleUrl: './category-bar.css'
})
export class CategoryBarComponent {
    categories = signal([
        { name: 'Iconic cities', icon: '🏙️' },
        { name: 'Amazing pools', icon: '🏊' },
        { name: 'Cabins', icon: '🏠' },
        { name: 'Beachfront', icon: '🏖️' },
        { name: 'Islands', icon: '🏝️' },
        { name: 'Lakefront', icon: '🌅' },
        { name: 'Castles', icon: '🏰' },
        { name: 'Arctic', icon: '❄️' },
        { name: 'Caves', icon: '🧗' },
        { name: 'Surfing', icon: '🏄' },
        { name: 'Bed & breakfasts', icon: '🍳' },
        { name: 'Tropical', icon: '🌴' },
        { name: 'National parks', icon: '🏞️' },
        { name: 'Vineyards', icon: '🍇' },
    ]);

    activeCategory = signal('Cabins');

    setActive(name: string) {
        this.activeCategory.set(name);
    }
}
