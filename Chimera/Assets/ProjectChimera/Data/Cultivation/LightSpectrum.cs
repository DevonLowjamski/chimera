using UnityEngine;

namespace ProjectChimera.Data.Cultivation
{
    /// <summary>
    /// Enumeration of light spectrum types used in cannabis cultivation.
    /// Different spectrums affect plant growth, flowering, and cannabinoid production.
    /// </summary>
    public enum LightSpectrum
    {
        // Full spectrum options
        FullSpectrum,           // Complete spectrum including UV and IR
        SunlightSpectrum,       // Natural sunlight spectrum
        
        // Vegetative growth spectrums
        BlueRich,               // High blue content for vegetative growth
        CoolWhite,              // 4000K-6500K for vegetative stage
        MetalHalide,            // Traditional MH spectrum
        
        // Flowering spectrums
        RedRich,                // High red content for flowering
        WarmWhite,              // 2700K-3500K for flowering stage
        HighPressureSodium,     // Traditional HPS spectrum
        
        // Specialized spectrums
        UVEnhanced,             // Enhanced UV for trichome production
        FarRedEnhanced,         // Enhanced far-red for stem elongation
        PurpleSpectrum,         // Blue + Red LED combination
        
        // Advanced LED spectrums
        QuantumBoard,           // Modern full-spectrum LED
        COBSpectrum,            // Chip-on-board LED spectrum
        CustomSpectrum          // User-defined spectrum
    }
} 