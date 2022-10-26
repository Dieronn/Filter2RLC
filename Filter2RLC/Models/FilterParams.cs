using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Filter2RLC.Models
{
    public class FilterParams
    {
        public int ID { get; set; }
        //---
        [Required(ErrorMessage = "You must enter a value for the Resistance field!")]
        [Range(0, 20)]
        [Display(Name = "Resistance R1 [Ohm]")]
        public double Resistance1 { get; set; }

        [Required(ErrorMessage = "You must enter a value for the Resistance field!")]
        [Range(0, 20)]
        [Display(Name = "Resistance R2 [Ohm]")]
        public double Resistance2 { get; set; }
        //---
        [Required(ErrorMessage = "You must enter a value for the Inductance field!")]
        [Range(0.000001, 10)]
        [Display(Name = "Inductance L1 [H]")]
        public double Inductance { get; set; }
        //---
        [Required(ErrorMessage = "You must enter a value for the Capacitance field!")]
        [Range(0.000001, 10)]
        [Display(Name = "Capacitance C1 [F]")]
        public double Capacitance { get; set; }
        //---
        [Required(ErrorMessage = "You must enter a value for the Voltage field!")]
        [Range(1, 500)]
        [Display(Name = "Amplitude of Input Voltage [V]")]
        public double U1 { get; set; }
        //---
        [Required(ErrorMessage = "You must enter a value for the minimum frequency field!")]
        [Range(0.1, 10)]
        [Display(Name = "Minimum Frequency [Hz]")]
        public double Fmin { get; set; }
        //---
        [Required(ErrorMessage = "You must enter a value for the maximum frequency field!")]
        [Range(10, 10000)]
        [Display(Name = "Maximum Frequency [Hz]")]
        public double Fmax { get; set; }
        //---
        [Required(ErrorMessage = "You must enter a value for the Number of Points field!")]
        [Range(100, 10000)]
        [Display(Name = "Number Of Points [-]")]
        public int NumOfRows { get; set; }
    }
}