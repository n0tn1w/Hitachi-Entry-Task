using System.ComponentModel.DataAnnotations;

namespace HitachiBE.Models.Database
{
    public class DayDataModel
    {
        public Guid Id { get; set; }

        public int Day { get; set; }

        public int Temperature { get; set; }

        public int Wind { get; set; }

        public int Humidity { get; set; }

        public int Precipitation { get; set; }

        public bool Lightning { get; set; }

        public string Clouds { get; set; }

        public int Score { get; set; }

    }
}
