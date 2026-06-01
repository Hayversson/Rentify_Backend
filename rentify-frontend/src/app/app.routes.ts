import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    title: 'Rentify | Home',
    loadComponent: () =>
      import('./features/home/home.page').then((m) => m.HomePageComponent)
  },
  {
    path: 'vehicles',
    title: 'Rentify | Vehicles',
    loadComponent: () =>
      import('./features/vehicles/vehicles.page').then((m) => m.VehiclesPageComponent)
  },
  {
    path: 'vehicles/:id',
    title: 'Rentify | Vehicle Details',
    loadComponent: () =>
      import('./features/vehicle-detail/vehicle-detail.page').then(
        (m) => m.VehicleDetailPageComponent
      )
  },
  {
    path: 'booking',
    title: 'Rentify | Booking',
    loadComponent: () =>
      import('./features/booking/booking.page').then((m) => m.BookingPageComponent)
  },
  {
    path: 'profile/history',
    title: 'Rentify | Your Rentals',
    loadComponent: () =>
      import('./features/profile-history/profile-history.page').then(
        (m) => m.ProfileHistoryPageComponent
      )
  },
  {
    path: '**',
    redirectTo: ''
  }
];
