import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

interface NavLink {
  label: string;
  path: string;
}

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
  protected readonly menuOpen = signal(false);
  protected readonly links: NavLink[] = [
    { label: 'Inicio', path: '/' },
    { label: 'Vehículos', path: '/vehicles' },
    { label: 'Reservas', path: '/booking' },
    { label: 'Historial', path: '/profile/history' }
  ];

  toggleMenu(): void {
    this.menuOpen.update((value) => !value);
  }
}
