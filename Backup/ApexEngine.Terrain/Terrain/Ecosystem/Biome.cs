using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Terrain.Ecosystem
{
    public class Biome
    {
        public enum BiomeTopography
        {
            Plains,
            Hills,
            Beach
        }

        private BiomeTopography topography;
        private float avgRainfall = 0f;
        private float avgTemperature = 20f; // 20 deg, celsius

        public Biome()
        {
            this.topography = BiomeTopography.Plains;
        }

        public Biome(BiomeTopography topography)
        {
            this.topography = topography;
        }

        public float AverageRainfall
        {
            get { return avgRainfall; }
            set { avgRainfall = value; }
        }

        public float AverageTemperature
        {
            get { return avgTemperature; }
            set { avgTemperature = value; }
        }
       

        public BiomeTopography Topography
        {
            get { return topography; }
            set { topography = value; }
        }

        public override string ToString()
        {
            return "Topography: " + topography.ToString();
        }
    }
}
