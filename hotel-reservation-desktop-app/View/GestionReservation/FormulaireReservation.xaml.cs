﻿using hotel_reservation_DAL.Contexts;
using hotel_reservation_DAL.Entities;
using hotel_reservation_desktop_app.ViewModels;
using System.Windows;


namespace hotel_reservation_desktop_app.View.GestionReservation
{
    public partial class FormulaireReservation : Window
    {
        public Reservation Reservation { get; private set; }

        public FormulaireReservation()
        {
            InitializeComponent();
            DataContext = new ReservationViewModel();

        }

    }

}