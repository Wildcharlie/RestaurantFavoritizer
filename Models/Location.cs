using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantFavoritizer.Models
{
    public class Location
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public string PhoneNo { get; set; }
        public string AddressLn1 { get; set; }
        public string AddressLn2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string ContentString { get; set; }
        public string Annotation { get; set; }
        [Index("LatLongIndex", 1, IsUnique = false)]
        public float Latitude { get; set; }
        [Index("LatLongIndex", 2, IsUnique = false)]
        public float Longitude { get; set; }
        public string MondayOpenHours { get; set; }
        public string TuesdayOpenHours { get; set; }
        public string WednesdayOpenHours { get; set; }
        public string ThursdayOpenHours { get; set; }
        public string FridayOpenHours { get; set; }
        public string SaturdayOpenHours { get; set; }
        public string SundayOpenHours { get; set; }
        public string GooglePlaceID { get; set; }
    }
}