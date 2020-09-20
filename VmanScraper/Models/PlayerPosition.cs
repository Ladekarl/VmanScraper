using System;
using System.Collections.Generic;

namespace VmanScraper.Models
{
    class PlayerPosition
    {
        public string Value { get; set; }
        private PlayerPosition(string value)
        {
            Value = value;
        }

        private static readonly ICollection<string> allowedPositions = new List<string>
        {
            "",
            "F",
            "FR",
            "FC",
            "FL",
            "M",
            "MD",
            "ML",
            "MR",
            "MC",
            "MO",
            "DL",
            "DC",
            "DR",
            "K"
        };

        public static PlayerPosition FromString(string position)
        {
            if (allowedPositions.Contains(position))
            {
                return new PlayerPosition(position);
            }
            else
            {
                throw new ArgumentException("Position not allowed");
            }
        }

        public static PlayerPosition Any
        {
            get { return new PlayerPosition(""); }
        }

        public static PlayerPosition Forward
        {
            get { return new PlayerPosition("F"); }
        }

        public static PlayerPosition ForwardRight
        {
            get { return new PlayerPosition("FR"); }
        }

        public static PlayerPosition ForwardCenter
        {
            get { return new PlayerPosition("FC"); }
        }

        public static PlayerPosition ForwardLeft
        {
            get { return new PlayerPosition("FL"); }
        }

        public static PlayerPosition Midfielder
        {
            get { return new PlayerPosition("M"); }
        }

        public static PlayerPosition MidfielderDefensive
        {
            get { return new PlayerPosition("MD"); }
        }

        public static PlayerPosition MidfielderLeft
        {
            get { return new PlayerPosition("ML"); }
        }

        public static PlayerPosition MidfielderRigt
        {
            get { return new PlayerPosition("MR"); }
        }

        public static PlayerPosition MidfielderCenter
        {
            get { return new PlayerPosition("MC"); }
        }

        public static PlayerPosition Defence
        {
            get { return new PlayerPosition("D"); }
        }

        public static PlayerPosition DefenceLeft
        {
            get { return new PlayerPosition("DL"); }
        }

        public static PlayerPosition DefenceRight
        {
            get { return new PlayerPosition("DR"); }
        }

        public static PlayerPosition DefenceCenter
        {
            get { return new PlayerPosition("DC"); }
        }

        public static PlayerPosition Keeper
        {
            get { return new PlayerPosition("K"); }
        }
    }
}
