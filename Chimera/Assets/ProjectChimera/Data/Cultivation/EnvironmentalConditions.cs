using UnityEngine;

namespace ProjectChimera.Data.Cultivation
{
    /// <summary>
    /// Represents environmental conditions that affect plant growth and development.
    /// Used throughout the cultivation and genetics systems for environmental calculations.
    /// </summary>
    [System.Serializable]
    public struct EnvironmentalConditions
    {
        [Header("Atmospheric Conditions")]
        [SerializeField, Range(-10f, 50f)] public float Temperature; // Celsius
        [SerializeField, Range(0f, 100f)] public float Humidity; // %RH
        [SerializeField, Range(200f, 2000f)] public float CO2Level; // ppm
        [SerializeField, Range(0f, 2f)] public float AirFlow; // Air movement factor

        [Header("Lighting")]
        [SerializeField, Range(0f, 1200f)] public float LightIntensity; // PPFD (Photosynthetic Photon Flux Density)
        [SerializeField, Range(0f, 24f)] public float PhotoperiodHours; // Hours of light per day
        [SerializeField, Range(2700f, 6500f)] public float ColorTemperature; // Kelvin

        [Header("Water and Nutrients")]
        [SerializeField, Range(0f, 100f)] public float WaterAvailability; // % availability
        [SerializeField, Range(4f, 8f)] public float pH; // pH level
        [SerializeField, Range(0f, 3000f)] public float ElectricalConductivity; // EC (μS/cm)

        [Header("Additional Factors")]
        [SerializeField, Range(0f, 1f)] public float OxygenLevel; // Root zone oxygen availability
        [SerializeField, Range(0f, 100f)] public float BarometricPressure; // kPa
        [SerializeField, Range(0f, 1f)] public float StressLevel; // Overall environmental stress (0 = no stress, 1 = maximum stress)

        /// <summary>
        /// Creates environmental conditions with typical indoor cultivation values.
        /// </summary>
        public static EnvironmentalConditions CreateIndoorDefault()
        {
            return new EnvironmentalConditions
            {
                Temperature = 24f,
                Humidity = 55f,
                CO2Level = 800f,
                AirFlow = 1f,
                LightIntensity = 400f,
                PhotoperiodHours = 18f,
                ColorTemperature = 4000f,
                WaterAvailability = 80f,
                pH = 6.5f,
                ElectricalConductivity = 1200f,
                OxygenLevel = 1f,
                BarometricPressure = 101.3f,
                StressLevel = 0f
            };
        }

        /// <summary>
        /// Creates environmental conditions with typical outdoor cultivation values.
        /// </summary>
        public static EnvironmentalConditions CreateOutdoorDefault()
        {
            return new EnvironmentalConditions
            {
                Temperature = 22f,
                Humidity = 65f,
                CO2Level = 400f,
                AirFlow = 1.5f,
                LightIntensity = 600f,
                PhotoperiodHours = 14f,
                ColorTemperature = 5500f,
                WaterAvailability = 70f,
                pH = 6.8f,
                ElectricalConductivity = 800f,
                OxygenLevel = 1f,
                BarometricPressure = 101.3f,
                StressLevel = 0.1f
            };
        }

        /// <summary>
        /// Creates environmental conditions representing stress conditions.
        /// </summary>
        public static EnvironmentalConditions CreateStressConditions()
        {
            return new EnvironmentalConditions
            {
                Temperature = 35f, // High temperature stress
                Humidity = 30f, // Low humidity stress
                CO2Level = 300f, // Low CO2
                AirFlow = 0.2f, // Poor air circulation
                LightIntensity = 150f, // Low light stress
                PhotoperiodHours = 12f,
                ColorTemperature = 3000f,
                WaterAvailability = 30f, // Drought stress
                pH = 8.2f, // High pH stress
                ElectricalConductivity = 2500f, // High nutrient concentration
                OxygenLevel = 0.3f, // Low oxygen stress
                BarometricPressure = 95f, // Low pressure
                StressLevel = 0.8f
            };
        }

        /// <summary>
        /// Calculates overall environmental suitability (0-1 scale, 1 = optimal).
        /// </summary>
        public float CalculateOverallSuitability()
        {
            // Define optimal ranges for general cannabis cultivation
            float tempScore = CalculateRangeScore(Temperature, 18f, 28f);
            float humidityScore = CalculateRangeScore(Humidity, 40f, 70f);
            float co2Score = CalculateRangeScore(CO2Level, 600f, 1200f);
            float lightScore = CalculateRangeScore(LightIntensity, 300f, 800f);
            float pHScore = CalculateRangeScore(pH, 6.0f, 7.0f);
            float waterScore = CalculateRangeScore(WaterAvailability, 60f, 90f);

            return (tempScore + humidityScore + co2Score + lightScore + pHScore + waterScore) / 6f;
        }

        /// <summary>
        /// Calculates stress level based on environmental conditions.
        /// </summary>
        public float CalculateEnvironmentalStress()
        {
            float suitability = CalculateOverallSuitability();
            float calculatedStress = 1f - suitability;
            
            // Combine with manually set stress level
            return Mathf.Max(calculatedStress, StressLevel);
        }

        /// <summary>
        /// Checks if conditions are within acceptable ranges for cannabis cultivation.
        /// </summary>
        public bool IsWithinAcceptableRanges()
        {
            return Temperature >= 15f && Temperature <= 35f &&
                   Humidity >= 20f && Humidity <= 85f &&
                   CO2Level >= 250f && CO2Level <= 2000f &&
                   LightIntensity >= 100f && LightIntensity <= 1200f &&
                   pH >= 5.0f && pH <= 8.5f &&
                   WaterAvailability >= 10f;
        }

        /// <summary>
        /// Interpolates between two environmental conditions.
        /// </summary>
        public static EnvironmentalConditions Lerp(EnvironmentalConditions a, EnvironmentalConditions b, float t)
        {
            return new EnvironmentalConditions
            {
                Temperature = Mathf.Lerp(a.Temperature, b.Temperature, t),
                Humidity = Mathf.Lerp(a.Humidity, b.Humidity, t),
                CO2Level = Mathf.Lerp(a.CO2Level, b.CO2Level, t),
                AirFlow = Mathf.Lerp(a.AirFlow, b.AirFlow, t),
                LightIntensity = Mathf.Lerp(a.LightIntensity, b.LightIntensity, t),
                PhotoperiodHours = Mathf.Lerp(a.PhotoperiodHours, b.PhotoperiodHours, t),
                ColorTemperature = Mathf.Lerp(a.ColorTemperature, b.ColorTemperature, t),
                WaterAvailability = Mathf.Lerp(a.WaterAvailability, b.WaterAvailability, t),
                pH = Mathf.Lerp(a.pH, b.pH, t),
                ElectricalConductivity = Mathf.Lerp(a.ElectricalConductivity, b.ElectricalConductivity, t),
                OxygenLevel = Mathf.Lerp(a.OxygenLevel, b.OxygenLevel, t),
                BarometricPressure = Mathf.Lerp(a.BarometricPressure, b.BarometricPressure, t),
                StressLevel = Mathf.Lerp(a.StressLevel, b.StressLevel, t)
            };
        }

        /// <summary>
        /// Creates a copy of the environmental conditions with modified values.
        /// </summary>
        public EnvironmentalConditions WithModifications(
            float? temperature = null,
            float? humidity = null,
            float? co2Level = null,
            float? lightIntensity = null,
            float? pH = null,
            float? waterAvailability = null,
            float? stressLevel = null)
        {
            return new EnvironmentalConditions
            {
                Temperature = temperature ?? Temperature,
                Humidity = humidity ?? Humidity,
                CO2Level = co2Level ?? CO2Level,
                AirFlow = AirFlow,
                LightIntensity = lightIntensity ?? LightIntensity,
                PhotoperiodHours = PhotoperiodHours,
                ColorTemperature = ColorTemperature,
                WaterAvailability = waterAvailability ?? WaterAvailability,
                pH = pH ?? this.pH,
                ElectricalConductivity = ElectricalConductivity,
                OxygenLevel = OxygenLevel,
                BarometricPressure = BarometricPressure,
                StressLevel = stressLevel ?? StressLevel
            };
        }

        private float CalculateRangeScore(float value, float min, float max)
        {
            if (value < min || value > max)
            {
                // Outside range - calculate how far outside
                float distance = Mathf.Min(Mathf.Abs(value - min), Mathf.Abs(value - max));
                float rangeSize = max - min;
                return Mathf.Max(0f, 1f - (distance / (rangeSize * 0.5f)));
            }
            
            // Inside range - perfect score
            return 1f;
        }

        public override string ToString()
        {
            return $"Environmental Conditions - Temp: {Temperature:F1}°C, Humidity: {Humidity:F1}%, " +
                   $"CO2: {CO2Level:F0}ppm, Light: {LightIntensity:F0}PPFD, pH: {pH:F1}, " +
                   $"Water: {WaterAvailability:F0}%, Stress: {StressLevel:F2}";
        }
    }
}