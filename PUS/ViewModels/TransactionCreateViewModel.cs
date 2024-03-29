﻿using MessagePack;
using PUS.Models;
using System.ComponentModel.DataAnnotations;

namespace PUS.ViewModels
{
    public class TransactionCreateViewModel
    {

        public string ServiceTitle { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane.")]
        public string OfferTo { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane.")]
        public string OfferBack { get; set; }
        public DateTime ExchangeDate { get; set; } = new DateTime(DateTime.Now.Ticks / 600000000 * 600000000);
        public int EQI { get; set; }
        public string? Remarks { get; set; }

        public string ClientId { get; set; }
        public int ServiceId { get; set; }
    }
}
