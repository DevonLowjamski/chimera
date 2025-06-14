using UnityEngine;
using ProjectChimera.Core;
using ProjectChimera.Data.Genetics;
using ProjectChimera.Systems.Cultivation;
using ProjectChimera.Systems.Progression;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

#if UNITY_SPEEDTREE
using SpeedTree;
#endif

namespace ProjectChimera.Systems.SpeedTree
{
    /// <summary>
    /// Advanced cannabis genetics engine providing comprehensive genetic simulation,
    /// breeding mechanics, and SpeedTree integration for realistic plant variation.
    /// Implements scientific cannabis genetics with polygenic traits, environmental
    /// adaptation, and sophisticated breeding algorithms.
    /// </summary>
    public class CannabisGeneticsEngine : ChimeraManager
    {
        [Header("Genetics Configuration")]
        [SerializeField] private CannabisGeneticsConfigSO _geneticsConfig;
        [SerializeField] private CannabisBreedingConfigSO _breedingConfig;
        [SerializeField] private CannabisGeneLibrarySO _geneLibrary;
        [SerializeField] private CannabisStrainDatabaseSO _strainDatabase;
        
        [Header("Genetic Simulation")]
        [SerializeField] private bool _enableMendelianInheritance = true;
        [SerializeField] private bool _enablePolygeneticTraits = true;
        [SerializeField] private bool _enableMutations = true;
        [SerializeField] private bool _enableEpigenetics = true;
        [SerializeField] private float _mutationRate = 0.001f;
        [SerializeField] private float _hybridVigorMultiplier = 1.2f;
        
        [Header("Environmental Genetics")]
        [SerializeField] private bool _enableEnvironmentalAdaptation = true;
        [SerializeField] private bool _enablePhenotypicPlasticity = true;
        [SerializeField] private float _adaptationRate = 0.01f;
        [SerializeField] private int _generationsForAdaptation = 5;
        
        [Header("Breeding System")]
        [SerializeField] private bool _enableAdvancedBreeding = true;
        [SerializeField] private bool _enableBackcrossing = true;
        [SerializeField] private bool _enableLineBreeding = true;
        [SerializeField] private int _maxBreedingGenerations = 10;
        
        // Core Genetics Systems
        private GeneticAlgorithmProcessor _geneticProcessor;
        private BreedingSimulator _breedingSimulator;
        private PhenotypicExpressionEngine _phenotypeEngine;
        private EnvironmentalAdaptationManager _adaptationManager;
        private GeneticDiversityTracker _diversityTracker;
        
        // Genetic Databases
        private Dictionary<string, CannabisGenotype> _genotypeDatabase = new Dictionary<string, CannabisGenotype>();
        private Dictionary<string, BreedingRecord> _breedingHistory = new Dictionary<string, BreedingRecord>();
        private Dictionary<string, GeneticLineage> _lineageTracker = new Dictionary<string, GeneticLineage>();
        private Dictionary<string, EnvironmentalAdaptation> _adaptationData = new Dictionary<string, EnvironmentalAdaptation>();
        
        // SpeedTree Integration
        private AdvancedSpeedTreeManager _speedTreeManager;
        private Dictionary<int, GeneticSpeedTreeMapping> _speedTreeMappings = new Dictionary<int, GeneticSpeedTreeMapping>();
        
        // Research Integration
        private ComprehensiveProgressionManager _progressionManager;
        private HashSet<string> _unlockedGenes = new HashSet<string>();
        private HashSet<string> _discoveredTraits = new HashSet<string>();
        
        // Performance and Analytics
        private GeneticsPerformanceMetrics _performanceMetrics;
        private GeneticsAnalytics _geneticsAnalytics;
        
        // Events
        public System.Action<CannabisGenotype> OnNewGenotypeGenerated;
        public System.Action<BreedingResult> OnBreedingCompleted;
        public System.Action<string> OnNewTraitDiscovered;
        public System.Action<string> OnGeneUnlocked;
        public System.Action<EnvironmentalAdaptation> OnAdaptationOccurred;
        
        // Properties
        public int TotalGenotypes => _genotypeDatabase.Count;
        public int TotalBreedingRecords => _breedingHistory.Count;
        public float GeneticDiversity => _diversityTracker.CalculateOverallDiversity();
        public GeneticsPerformanceMetrics PerformanceMetrics => _performanceMetrics;
        
        protected override void InitializeManager()
        {
            InitializeGeneticsSystems();
            InitializeGeneticDatabases();
            ConnectToGameSystems();
            InitializePerformanceMonitoring();
            StartGeneticsUpdateLoop();
            LogInfo("Cannabis Genetics Engine initialized");
        }
        
        private void Update()
        {
            UpdateGeneticSystems();
            UpdateEnvironmentalAdaptation();
            UpdatePerformanceMetrics();
        }
        
        #region Initialization
        
        private void InitializeGeneticsSystems()
        {
            // Initialize core genetics processors
            _geneticProcessor = new GeneticAlgorithmProcessor(_geneticsConfig);
            _breedingSimulator = new BreedingSimulator(_breedingConfig);
            _phenotypeEngine = new PhenotypicExpressionEngine(_geneLibrary);
            _adaptationManager = new EnvironmentalAdaptationManager(_enableEnvironmentalAdaptation);
            _diversityTracker = new GeneticDiversityTracker();
            
            // Initialize analytics
            _geneticsAnalytics = new GeneticsAnalytics();
            
            LogInfo("Genetics systems initialized");
        }
        
        private void InitializeGeneticDatabases()
        {
            // Load base strains from database
            if (_strainDatabase != null)
            {
                var baseStrains = _strainDatabase.GetAllBaseStrains();
                foreach (var strain in baseStrains)
                {
                    var genotype = CreateBaseGenotype(strain);
                    _genotypeDatabase[genotype.GenotypeId] = genotype;
                }
                
                LogInfo($"Loaded {baseStrains.Count} base strain genotypes");
            }
            
            // Initialize gene library
            if (_geneLibrary != null)
            {
                _geneLibrary.InitializeGeneDatabase();
                LogInfo($"Gene library initialized with {_geneLibrary.GetTotalGenes()} genes");
            }
        }
        
        private CannabisGenotype CreateBaseGenotype(CannabisStrainAsset strain)
        {
            var genotype = new CannabisGenotype
            {
                GenotypeId = Guid.NewGuid().ToString(),
                StrainId = strain.StrainId,
                StrainName = strain.StrainName,
                Generation = 0,
                CreationDate = DateTime.Now,
                IsFounderStrain = true
            };
            
            // Generate alleles for all genes
            GenerateFounderAlleles(genotype, strain);
            
            // Calculate genetic diversity
            _diversityTracker.AddGenotype(genotype);
            
            return genotype;
        }
        
        private void GenerateFounderAlleles(CannabisGenotype genotype, CannabisStrainAsset strain)
        {
            var allGenes = _geneLibrary.GetAllGenes();
            
            foreach (var gene in allGenes)
            {
                var alleles = GenerateFounderAllelesForGene(gene, strain);
                genotype.Alleles[gene.GeneId] = alleles;
            }
        }
        
        private List<Allele> GenerateFounderAllelesForGene(GeneDefinition gene, CannabisStrainAsset strain)
        {
            var alleles = new List<Allele>();
            
            // Generate two alleles (diploid)
            for (int i = 0; i < 2; i++)
            {
                var allele = CreateAlleleFromStrain(gene, strain);
                alleles.Add(allele);
            }
            
            return alleles;
        }
        
        private Allele CreateAlleleFromStrain(GeneDefinition gene, CannabisStrainAsset strain)
        {
            var allele = new Allele
            {
                AlleleId = Guid.NewGuid().ToString(),
                GeneId = gene.GeneId,
                AlleleName = $"{gene.GeneName}_{strain.StrainName}",
                Dominance = UnityEngine.Random.Range(0f, 1f),
                Expression = CalculateStrainBasedExpression(gene, strain),
                Stability = UnityEngine.Random.Range(0.8f, 1f),
                MutationRate = _mutationRate
            };
            
            // Set allele-specific properties based on gene type and strain
            SetAllelePropertiesFromStrain(allele, gene, strain);
            
            return allele;
        }
        
        private float CalculateStrainBasedExpression(GeneDefinition gene, CannabisStrainAsset strain)
        {
            // Map strain characteristics to gene expression
            return gene.GeneType switch
            {
                GeneType.PlantHeight => Mathf.Lerp(strain.HeightRange.x, strain.HeightRange.y, 0.5f) / 3f, // Normalize
                GeneType.LeafSize => strain.LeafSize,
                GeneType.BudDensity => strain.BudDensity,
                GeneType.TrichromeProduction => strain.TrichromeAmount,
                GeneType.FloweringTime => strain.FloweringTimeRange.x / 15f, // Normalize weeks to 0-1
                GeneType.YieldPotential => strain.YieldRange.x / 600f, // Normalize grams to 0-1
                GeneType.THCProduction => UnityEngine.Random.Range(0.1f, 0.3f), // Base THC potential
                GeneType.CBDProduction => UnityEngine.Random.Range(0.01f, 0.2f), // Base CBD potential
                GeneType.TerpeneProfile => UnityEngine.Random.Range(0.3f, 0.8f), // Terpene diversity
                GeneType.DiseaseResistance => UnityEngine.Random.Range(0.4f, 0.9f),
                GeneType.EnvironmentalTolerance => UnityEngine.Random.Range(0.3f, 0.8f),
                _ => UnityEngine.Random.Range(0.3f, 0.7f)
            };
        }
        
        private void SetAllelePropertiesFromStrain(Allele allele, GeneDefinition gene, CannabisStrainAsset strain)
        {
            // Set color properties for visual genes
            if (gene.GeneType == GeneType.LeafColor)
            {
                allele.ColorValue = strain.LeafColorBase;
            }
            else if (gene.GeneType == GeneType.BudColor)
            {
                allele.ColorValue = strain.BudColorBase;
            }
            
            // Set morphological properties
            if (gene.GeneType == GeneType.PlantMorphology)
            {
                allele.MorphologyValue = strain.Morphology;
            }
            
            // Set strain type influences
            allele.StrainTypeInfluence = new Dictionary<CannabisStrainType, float>
            {
                [CannabisStrainType.Indica] = strain.IndicaDominance,
                [CannabisStrainType.Sativa] = strain.SativaDominance,
                [CannabisStrainType.Ruderalis] = strain.RuderalisInfluence
            };
        }
        
        private void ConnectToGameSystems()
        {
            if (GameManager.Instance != null)
            {
                _speedTreeManager = GameManager.Instance.GetManager<AdvancedSpeedTreeManager>();
                _progressionManager = GameManager.Instance.GetManager<ComprehensiveProgressionManager>();
            }
            
            ConnectSystemEvents();
        }
        
        private void ConnectSystemEvents()
        {
            if (_speedTreeManager != null)
            {
                _speedTreeManager.OnPlantInstanceCreated += HandlePlantInstanceCreated;
            }
            
            if (_progressionManager != null)
            {
                _progressionManager.OnResearchCompleted += HandleResearchCompleted;
            }
        }
        
        private void InitializePerformanceMonitoring()
        {
            _performanceMetrics = new GeneticsPerformanceMetrics
            {
                TotalGenotypes = 0,
                TotalGenes = _geneLibrary?.GetTotalGenes() ?? 0,
                AverageGeneticDiversity = 0f,
                BreedingOperationsPerSecond = 0f
            };
        }
        
        private void StartGeneticsUpdateLoop()
        {
            StartCoroutine(GeneticsUpdateCoroutine());
        }
        
        private IEnumerator GeneticsUpdateCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                
                if (_enableEnvironmentalAdaptation)
                {
                    ProcessEnvironmentalAdaptation();
                }
                
                if (_enableEpigenetics)
                {
                    ProcessEpigeneticChanges();
                }
                
                UpdateGeneticDiversity();
                ProcessBreedingQueue();
            }
        }
        
        #endregion
        
        #region Genetic Generation and Variation
        
        public CannabisGenotype GenerateGeneticVariation(string baseStrainId, EnvironmentalConditions? conditions = null)
        {
            if (!_genotypeDatabase.TryGetValue(baseStrainId, out var baseGenotype))
            {
                LogWarning($"Base genotype not found: {baseStrainId}");
                return null;
            }
            
            var newGenotype = new CannabisGenotype
            {
                GenotypeId = Guid.NewGuid().ToString(),
                StrainId = baseGenotype.StrainId,
                StrainName = $"{baseGenotype.StrainName}_Var_{_genotypeDatabase.Count}",
                Generation = baseGenotype.Generation + 1,
                CreationDate = DateTime.Now,
                ParentGenotypes = new List<string> { baseStrainId }
            };
            
            // Generate genetic variation through multiple mechanisms
            ApplyGeneticVariation(newGenotype, baseGenotype);
            
            if (_enableMutations)
            {
                ApplyMutations(newGenotype);
            }
            
            if (conditions.HasValue && _enableEnvironmentalAdaptation)
            {
                ApplyEnvironmentalPressure(newGenotype, conditions.Value);
            }
            
            // Calculate phenotype from genotype
            var phenotype = _phenotypeEngine.CalculatePhenotype(newGenotype);
            newGenotype.ExpressedPhenotype = phenotype;
            
            // Store in database
            _genotypeDatabase[newGenotype.GenotypeId] = newGenotype;
            _diversityTracker.AddGenotype(newGenotype);
            
            OnNewGenotypeGenerated?.Invoke(newGenotype);
            
            LogInfo($"Generated genetic variation: {newGenotype.StrainName}");
            return newGenotype;
        }
        
        private void ApplyGeneticVariation(CannabisGenotype newGenotype, CannabisGenotype baseGenotype)
        {
            foreach (var geneAlleles in baseGenotype.Alleles)
            {
                var geneId = geneAlleles.Key;
                var baseAlleles = geneAlleles.Value;
                
                var newAlleles = new List<Allele>();
                
                // Apply variation to each allele
                foreach (var baseAllele in baseAlleles)
                {
                    var newAllele = CreateVariantAllele(baseAllele);
                    newAlleles.Add(newAllele);
                }
                
                newGenotype.Alleles[geneId] = newAlleles;
            }
        }
        
        private Allele CreateVariantAllele(Allele baseAllele)
        {
            var variant = new Allele
            {
                AlleleId = Guid.NewGuid().ToString(),
                GeneId = baseAllele.GeneId,
                AlleleName = $"{baseAllele.AlleleName}_Var",
                Dominance = baseAllele.Dominance + UnityEngine.Random.Range(-0.1f, 0.1f),
                Expression = baseAllele.Expression + UnityEngine.Random.Range(-0.05f, 0.05f),
                Stability = baseAllele.Stability,
                MutationRate = baseAllele.MutationRate,
                ColorValue = VaryColor(baseAllele.ColorValue),
                MorphologyValue = baseAllele.MorphologyValue,
                StrainTypeInfluence = new Dictionary<CannabisStrainType, float>(baseAllele.StrainTypeInfluence)
            };
            
            // Clamp values
            variant.Dominance = Mathf.Clamp01(variant.Dominance);
            variant.Expression = Mathf.Clamp01(variant.Expression);
            
            return variant;
        }
        
        private Color VaryColor(Color baseColor)
        {
            float hue, saturation, value;
            Color.RGBToHSV(baseColor, out hue, out saturation, out value);
            
            hue += UnityEngine.Random.Range(-0.05f, 0.05f);
            saturation += UnityEngine.Random.Range(-0.1f, 0.1f);
            value += UnityEngine.Random.Range(-0.1f, 0.1f);
            
            hue = Mathf.Repeat(hue, 1f);
            saturation = Mathf.Clamp01(saturation);
            value = Mathf.Clamp01(value);
            
            return Color.HSVToRGB(hue, saturation, value);
        }
        
        private void ApplyMutations(CannabisGenotype genotype)
        {
            foreach (var geneAlleles in genotype.Alleles.ToList())
            {
                foreach (var allele in geneAlleles.Value)
                {
                    if (UnityEngine.Random.value < allele.MutationRate)
                    {
                        ApplyMutation(allele);
                    }
                }
            }
        }
        
        private void ApplyMutation(Allele allele)
        {
            var mutationType = (MutationType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(MutationType)).Length);
            
            switch (mutationType)
            {
                case MutationType.PointMutation:
                    allele.Expression += UnityEngine.Random.Range(-0.1f, 0.1f);
                    allele.Expression = Mathf.Clamp01(allele.Expression);
                    break;
                
                case MutationType.DominanceShift:
                    allele.Dominance += UnityEngine.Random.Range(-0.2f, 0.2f);
                    allele.Dominance = Mathf.Clamp01(allele.Dominance);
                    break;
                
                case MutationType.StabilityChange:
                    allele.Stability += UnityEngine.Random.Range(-0.1f, 0.1f);
                    allele.Stability = Mathf.Clamp(allele.Stability, 0.1f, 1f);
                    break;
                
                case MutationType.ColorMutation:
                    if (allele.ColorValue != Color.clear)
                    {
                        allele.ColorValue = VaryColor(allele.ColorValue);
                    }
                    break;
            }
            
            LogInfo($"Mutation applied to allele {allele.AlleleId}: {mutationType}");
        }
        
        private void ApplyEnvironmentalPressure(CannabisGenotype genotype, EnvironmentalConditions conditions)
        {
            // Environmental pressure affects allele expression and stability
            foreach (var geneAlleles in genotype.Alleles)
            {
                var gene = _geneLibrary.GetGene(geneAlleles.Key);
                if (gene == null) continue;
                
                var environmentalFactor = CalculateEnvironmentalFactor(gene, conditions);
                
                foreach (var allele in geneAlleles.Value)
                {
                    // Favorable conditions increase expression and stability
                    if (environmentalFactor > 0.7f)
                    {
                        allele.Expression *= 1.05f;
                        allele.Stability *= 1.02f;
                    }
                    // Harsh conditions decrease expression and stability
                    else if (environmentalFactor < 0.3f)
                    {
                        allele.Expression *= 0.95f;
                        allele.Stability *= 0.98f;
                    }
                    
                    allele.Expression = Mathf.Clamp01(allele.Expression);
                    allele.Stability = Mathf.Clamp(allele.Stability, 0.1f, 1f);
                }
            }
        }
        
        private float CalculateEnvironmentalFactor(GeneDefinition gene, EnvironmentalConditions conditions)
        {
            // Calculate how favorable the environment is for this gene
            float factor = 0.5f; // Neutral
            
            switch (gene.GeneType)
            {
                case GeneType.EnvironmentalTolerance:
                    // Temperature tolerance
                    if (conditions.Temperature >= 20f && conditions.Temperature <= 28f)
                        factor += 0.3f;
                    else
                        factor -= 0.2f;
                    break;
                
                case GeneType.PlantHeight:
                    // Light intensity affects height
                    if (conditions.LightIntensity >= 600f && conditions.LightIntensity <= 1000f)
                        factor += 0.2f;
                    break;
                
                case GeneType.TrichromeProduction:
                    // Stress can increase trichrome production
                    if (conditions.Temperature > 26f || conditions.Humidity < 50f)
                        factor += 0.1f;
                    break;
            }
            
            return Mathf.Clamp01(factor);
        }
        
        #endregion
        
        #region Breeding System
        
        public BreedingResult PerformBreeding(string parent1Id, string parent2Id, BreedingMethod method = BreedingMethod.StandardCross)
        {
            if (!_genotypeDatabase.TryGetValue(parent1Id, out var parent1) ||
                !_genotypeDatabase.TryGetValue(parent2Id, out var parent2))
            {
                LogError("One or both parent genotypes not found");
                return null;
            }
            
            var breedingResult = _breedingSimulator.PerformBreeding(parent1, parent2, method);
            
            if (breedingResult.Success)
            {
                // Generate offspring genotypes
                var offspring = new List<CannabisGenotype>();
                
                for (int i = 0; i < breedingResult.OffspringCount; i++)
                {
                    var child = GenerateOffspring(parent1, parent2, method);
                    offspring.Add(child);
                    _genotypeDatabase[child.GenotypeId] = child;
                }
                
                breedingResult.OffspringGenotypes = offspring;
                
                // Record breeding history
                var breedingRecord = new BreedingRecord
                {
                    BreedingId = Guid.NewGuid().ToString(),
                    Parent1Id = parent1Id,
                    Parent2Id = parent2Id,
                    Method = method,
                    BreedingDate = DateTime.Now,
                    OffspringIds = offspring.Select(o => o.GenotypeId).ToList(),
                    Success = true,
                    Notes = $"Bred {parent1.StrainName} x {parent2.StrainName}"
                };
                
                _breedingHistory[breedingRecord.BreedingId] = breedingRecord;
                
                // Update lineage tracking
                foreach (var child in offspring)
                {
                    UpdateLineage(child, parent1, parent2);
                }
                
                OnBreedingCompleted?.Invoke(breedingResult);
                
                LogInfo($"Breeding successful: {offspring.Count} offspring generated");
            }
            
            return breedingResult;
        }
        
        private CannabisGenotype GenerateOffspring(CannabisGenotype parent1, CannabisGenotype parent2, BreedingMethod method)
        {
            var offspring = new CannabisGenotype
            {
                GenotypeId = Guid.NewGuid().ToString(),
                StrainId = $"Hybrid_{Guid.NewGuid().ToString()[..8]}",
                StrainName = $"{parent1.StrainName} x {parent2.StrainName}",
                Generation = Mathf.Max(parent1.Generation, parent2.Generation) + 1,
                CreationDate = DateTime.Now,
                ParentGenotypes = new List<string> { parent1.GenotypeId, parent2.GenotypeId },
                IsHybrid = true
            };
            
            // Genetic recombination
            PerformGeneticRecombination(offspring, parent1, parent2, method);
            
            // Apply hybrid vigor if applicable
            if (method == BreedingMethod.StandardCross && IsOutcross(parent1, parent2))
            {
                ApplyHybridVigor(offspring);
            }
            
            // Calculate phenotype
            offspring.ExpressedPhenotype = _phenotypeEngine.CalculatePhenotype(offspring);
            
            return offspring;
        }
        
        private void PerformGeneticRecombination(CannabisGenotype offspring, CannabisGenotype parent1, CannabisGenotype parent2, BreedingMethod method)
        {
            var allGenes = _geneLibrary.GetAllGenes();
            
            foreach (var gene in allGenes)
            {
                var offspringAlleles = new List<Allele>();
                
                // Get alleles from both parents
                var parent1Alleles = parent1.Alleles.GetValueOrDefault(gene.GeneId, new List<Allele>());
                var parent2Alleles = parent2.Alleles.GetValueOrDefault(gene.GeneId, new List<Allele>());
                
                if (parent1Alleles.Count == 0 || parent2Alleles.Count == 0)
                {
                    // Generate default alleles if missing
                    offspringAlleles = GenerateDefaultAlleles(gene);
                }
                else
                {
                    // Mendelian inheritance with crossover
                    if (_enableMendelianInheritance)
                    {
                        offspringAlleles = PerformMendelianInheritance(parent1Alleles, parent2Alleles, gene);
                    }
                    else
                    {
                        // Simple random selection
                        offspringAlleles.Add(SelectRandomAllele(parent1Alleles));
                        offspringAlleles.Add(SelectRandomAllele(parent2Alleles));
                    }
                }
                
                offspring.Alleles[gene.GeneId] = offspringAlleles;
            }
        }
        
        private List<Allele> PerformMendelianInheritance(List<Allele> parent1Alleles, List<Allele> parent2Alleles, GeneDefinition gene)
        {
            var offspring = new List<Allele>();
            
            // Select one allele from each parent (simulating meiosis)
            var allele1 = SelectRandomAllele(parent1Alleles);
            var allele2 = SelectRandomAllele(parent2Alleles);
            
            // Create new allele instances (to avoid reference issues)
            var newAllele1 = CloneAllele(allele1);
            var newAllele2 = CloneAllele(allele2);
            
            // Apply recombination (crossover) if applicable
            if (gene.AllowsRecombination && UnityEngine.Random.value < gene.RecombinationRate)
            {
                PerformCrossover(newAllele1, newAllele2);
            }
            
            offspring.Add(newAllele1);
            offspring.Add(newAllele2);
            
            return offspring;
        }
        
        private Allele SelectRandomAllele(List<Allele> alleles)
        {
            return alleles[UnityEngine.Random.Range(0, alleles.Count)];
        }
        
        private Allele CloneAllele(Allele original)
        {
            return new Allele
            {
                AlleleId = Guid.NewGuid().ToString(),
                GeneId = original.GeneId,
                AlleleName = original.AlleleName,
                Dominance = original.Dominance,
                Expression = original.Expression,
                Stability = original.Stability,
                MutationRate = original.MutationRate,
                ColorValue = original.ColorValue,
                MorphologyValue = original.MorphologyValue,
                StrainTypeInfluence = new Dictionary<CannabisStrainType, float>(original.StrainTypeInfluence)
            };
        }
        
        private void PerformCrossover(Allele allele1, Allele allele2)
        {
            // Exchange some properties between alleles
            var tempExpression = allele1.Expression;
            allele1.Expression = (allele1.Expression + allele2.Expression) / 2f;
            allele2.Expression = (tempExpression + allele2.Expression) / 2f;
            
            // Exchange color components
            if (allele1.ColorValue != Color.clear && allele2.ColorValue != Color.clear)
            {
                var tempColor = allele1.ColorValue;
                allele1.ColorValue = Color.Lerp(allele1.ColorValue, allele2.ColorValue, 0.5f);
                allele2.ColorValue = Color.Lerp(tempColor, allele2.ColorValue, 0.5f);
            }
        }
        
        private List<Allele> GenerateDefaultAlleles(GeneDefinition gene)
        {
            var alleles = new List<Allele>();
            
            for (int i = 0; i < 2; i++)
            {
                var allele = new Allele
                {
                    AlleleId = Guid.NewGuid().ToString(),
                    GeneId = gene.GeneId,
                    AlleleName = $"{gene.GeneName}_Default_{i}",
                    Dominance = UnityEngine.Random.value,
                    Expression = UnityEngine.Random.Range(0.3f, 0.7f),
                    Stability = 0.8f,
                    MutationRate = _mutationRate
                };
                
                alleles.Add(allele);
            }
            
            return alleles;
        }
        
        private bool IsOutcross(CannabisGenotype parent1, CannabisGenotype parent2)
        {
            // Determine if this is an outcross (unrelated parents) vs inbreeding
            var lineage1 = GetLineage(parent1.GenotypeId);
            var lineage2 = GetLineage(parent2.GenotypeId);
            
            // Check for shared ancestors in recent generations
            var sharedAncestors = lineage1.Ancestors.Intersect(lineage2.Ancestors).ToList();
            
            return sharedAncestors.Count == 0 || !HasRecentSharedAncestors(sharedAncestors, 3);
        }
        
        private bool HasRecentSharedAncestors(List<string> sharedAncestors, int generations)
        {
            // Implementation would check generation distance of shared ancestors
            return false; // Simplified for now
        }
        
        private void ApplyHybridVigor(CannabisGenotype offspring)
        {
            // Hybrid vigor (heterosis) increases overall performance
            foreach (var geneAlleles in offspring.Alleles)
            {
                foreach (var allele in geneAlleles.Value)
                {
                    allele.Expression *= _hybridVigorMultiplier;
                    allele.Expression = Mathf.Clamp01(allele.Expression);
                }
            }
            
            offspring.HybridVigor = _hybridVigorMultiplier;
            LogInfo($"Applied hybrid vigor to {offspring.StrainName}: {_hybridVigorMultiplier}x");
        }
        
        private void UpdateLineage(CannabisGenotype offspring, CannabisGenotype parent1, CannabisGenotype parent2)
        {
            var lineage = new GeneticLineage
            {
                GenotypeId = offspring.GenotypeId,
                DirectParents = new List<string> { parent1.GenotypeId, parent2.GenotypeId },
                Ancestors = new List<string>()
            };
            
            // Collect ancestors from both parents
            var parent1Lineage = GetLineage(parent1.GenotypeId);
            var parent2Lineage = GetLineage(parent2.GenotypeId);
            
            lineage.Ancestors.AddRange(parent1Lineage.Ancestors);
            lineage.Ancestors.AddRange(parent2Lineage.Ancestors);
            lineage.Ancestors.Add(parent1.GenotypeId);
            lineage.Ancestors.Add(parent2.GenotypeId);
            
            // Remove duplicates and limit depth
            lineage.Ancestors = lineage.Ancestors.Distinct().Take(20).ToList();
            
            _lineageTracker[offspring.GenotypeId] = lineage;
        }
        
        private GeneticLineage GetLineage(string genotypeId)
        {
            return _lineageTracker.GetValueOrDefault(genotypeId, new GeneticLineage { GenotypeId = genotypeId });
        }
        
        #endregion
        
        #region SpeedTree Integration
        
        public void ApplyGeneticsToSpeedTree(SpeedTreePlantInstance instance, CannabisGenotype genotype)
        {
            if (instance?.Renderer == null || genotype == null) return;
            
            var phenotype = genotype.ExpressedPhenotype ?? _phenotypeEngine.CalculatePhenotype(genotype);
            
            // Create mapping for tracking
            var mapping = new GeneticSpeedTreeMapping
            {
                InstanceId = instance.InstanceId,
                GenotypeId = genotype.GenotypeId,
                PhenotypeSnapshot = phenotype,
                LastUpdate = Time.time
            };
            
            _speedTreeMappings[instance.InstanceId] = mapping;
            
            // Apply genetic traits to SpeedTree renderer
            ApplyMorphologicalTraits(instance, phenotype);
            ApplyColorTraits(instance, phenotype);
            ApplyGrowthTraits(instance, phenotype);
            ApplyEnvironmentalTraits(instance, phenotype);
            
            LogInfo($"Applied genetics to SpeedTree instance {instance.InstanceId}");
        }
        
        private void ApplyMorphologicalTraits(SpeedTreePlantInstance instance, CannabisPhenotype phenotype)
        {
#if UNITY_SPEEDTREE
            if (instance.Renderer?.materialProperties == null) return;
            
            // Plant size and structure
            var sizeMultiplier = phenotype.PlantHeight * phenotype.PlantWidth;
            instance.Renderer.transform.localScale = Vector3.one * sizeMultiplier;
            
            // Branch characteristics
            instance.Renderer.materialProperties.SetFloat("_BranchDensity", phenotype.BranchDensity);
            instance.Renderer.materialProperties.SetFloat("_InternodeSpacing", phenotype.InternodeSpacing);
            instance.Renderer.materialProperties.SetFloat("_StemThickness", phenotype.StemThickness);
            
            // Leaf characteristics
            instance.Renderer.materialProperties.SetFloat("_LeafSize", phenotype.LeafSize);
            instance.Renderer.materialProperties.SetFloat("_LeafDensity", phenotype.LeafDensity);
            instance.Renderer.materialProperties.SetFloat("_LeafSerration", phenotype.LeafSerration);
            instance.Renderer.materialProperties.SetInt("_LeafletCount", phenotype.LeafletCount);
            
            // Bud characteristics
            instance.Renderer.materialProperties.SetFloat("_BudDensity", phenotype.BudDensity);
            instance.Renderer.materialProperties.SetFloat("_BudSize", phenotype.BudSize);
            instance.Renderer.materialProperties.SetFloat("_CalyxSize", phenotype.CalyxSize);
            
            // Trichrome characteristics
            instance.Renderer.materialProperties.SetFloat("_TrichromeAmount", phenotype.TrichromeAmount);
            instance.Renderer.materialProperties.SetFloat("_TrichromeSize", phenotype.TrichromeSize);
            instance.Renderer.materialProperties.SetColor("_TrichromeColor", phenotype.TrichromeColor);
#endif
        }
        
        private void ApplyColorTraits(SpeedTreePlantInstance instance, CannabisPhenotype phenotype)
        {
#if UNITY_SPEEDTREE
            if (instance.Renderer?.materialProperties == null) return;
            
            // Leaf colors
            instance.Renderer.materialProperties.SetColor("_LeafColorPrimary", phenotype.LeafColorPrimary);
            instance.Renderer.materialProperties.SetColor("_LeafColorSecondary", phenotype.LeafColorSecondary);
            instance.Renderer.materialProperties.SetFloat("_LeafColorVariation", phenotype.LeafColorVariation);
            
            // Bud colors
            instance.Renderer.materialProperties.SetColor("_BudColorPrimary", phenotype.BudColorPrimary);
            instance.Renderer.materialProperties.SetColor("_BudColorSecondary", phenotype.BudColorSecondary);
            instance.Renderer.materialProperties.SetColor("_PistilColor", phenotype.PistilColor);
            
            // Stem color
            instance.Renderer.materialProperties.SetColor("_StemColor", phenotype.StemColor);
            
            // Apply seasonal color changes if applicable
            if (phenotype.SeasonalColorChange)
            {
                instance.Renderer.materialProperties.SetFloat("_SeasonalColorIntensity", phenotype.SeasonalColorIntensity);
            }
#endif
        }
        
        private void ApplyGrowthTraits(SpeedTreePlantInstance instance, CannabisPhenotype phenotype)
        {
            // Set growth parameters
            instance.GeneticData = new CannabisGeneticData
            {
                PlantSize = phenotype.PlantHeight,
                GrowthRate = phenotype.GrowthRate,
                FloweringSpeed = phenotype.FloweringSpeed,
                YieldPotential = phenotype.YieldPotential
            };
            
            // Update growth modifiers
            instance.EnvironmentalModifiers["genetic_growth_rate"] = phenotype.GrowthRate;
            instance.EnvironmentalModifiers["genetic_flowering_speed"] = phenotype.FloweringSpeed;
        }
        
        private void ApplyEnvironmentalTraits(SpeedTreePlantInstance instance, CannabisPhenotype phenotype)
        {
            // Environmental tolerance affects stress response
            instance.GeneticData.HeatTolerance = phenotype.HeatTolerance;
            instance.GeneticData.ColdTolerance = phenotype.ColdTolerance;
            instance.GeneticData.DroughtTolerance = phenotype.DroughtTolerance;
            instance.GeneticData.MoldResistance = phenotype.MoldResistance;
            instance.GeneticData.PestResistance = phenotype.PestResistance;
            
            // Wind response characteristics
            instance.GeneticData.WindResistance = phenotype.WindResistance;
            instance.GeneticData.BranchFlexibility = phenotype.BranchFlexibility;
            instance.GeneticData.StemStiffness = phenotype.StemStiffness;
        }
        
        #endregion
        
        #region Environmental Adaptation
        
        private void ProcessEnvironmentalAdaptation()
        {
            foreach (var mapping in _speedTreeMappings.Values)
            {
                if (_genotypeDatabase.TryGetValue(mapping.GenotypeId, out var genotype))
                {
                    ProcessGenotypeAdaptation(genotype, mapping);
                }
            }
        }
        
        private void ProcessGenotypeAdaptation(CannabisGenotype genotype, GeneticSpeedTreeMapping mapping)
        {
            // Get current environmental conditions for this plant
            var conditions = GetEnvironmentalConditionsForPlant(mapping.InstanceId);
            if (!conditions.HasValue) return;
            
            var adaptationKey = $"{genotype.GenotypeId}_{GetEnvironmentalHash(conditions.Value)}";
            
            if (!_adaptationData.TryGetValue(adaptationKey, out var adaptation))
            {
                adaptation = new EnvironmentalAdaptation
                {
                    GenotypeId = genotype.GenotypeId,
                    EnvironmentalConditions = conditions.Value,
                    StartDate = DateTime.Now,
                    AdaptationProgress = 0f
                };
                
                _adaptationData[adaptationKey] = adaptation;
            }
            
            // Update adaptation progress
            adaptation.AdaptationProgress += _adaptationRate * Time.deltaTime;
            adaptation.AdaptationProgress = Mathf.Clamp01(adaptation.AdaptationProgress);
            
            // Apply adaptive changes when threshold reached
            if (adaptation.AdaptationProgress > 0.5f && !adaptation.AdaptationApplied)
            {
                ApplyAdaptiveChanges(genotype, adaptation);
                adaptation.AdaptationApplied = true;
                OnAdaptationOccurred?.Invoke(adaptation);
            }
        }
        
        private EnvironmentalConditions? GetEnvironmentalConditionsForPlant(int instanceId)
        {
            // This would interface with the environmental manager to get local conditions
            return new EnvironmentalConditions
            {
                Temperature = 24f + UnityEngine.Random.Range(-3f, 3f),
                Humidity = 60f + UnityEngine.Random.Range(-10f, 10f),
                LightIntensity = 800f + UnityEngine.Random.Range(-200f, 200f),
                CO2Level = 400f + UnityEngine.Random.Range(-50f, 100f)
            };
        }
        
        private string GetEnvironmentalHash(EnvironmentalConditions conditions)
        {
            // Create a hash representing the environmental conditions
            var tempBucket = Mathf.FloorToInt(conditions.Temperature / 5f) * 5; // 5-degree buckets
            var humidityBucket = Mathf.FloorToInt(conditions.Humidity / 10f) * 10; // 10% buckets
            var lightBucket = Mathf.FloorToInt(conditions.LightIntensity / 100f) * 100; // 100 PPFD buckets
            
            return $"T{tempBucket}H{humidityBucket}L{lightBucket}";
        }
        
        private void ApplyAdaptiveChanges(CannabisGenotype genotype, EnvironmentalAdaptation adaptation)
        {
            var conditions = adaptation.EnvironmentalConditions;
            
            // Adapt to temperature
            if (conditions.Temperature > 28f)
            {
                // Increase heat tolerance genes
                AdaptGeneExpression(genotype, GeneType.EnvironmentalTolerance, 0.1f);
                AdaptGeneExpression(genotype, GeneType.LeafSize, -0.05f); // Smaller leaves for heat
            }
            else if (conditions.Temperature < 20f)
            {
                // Increase cold tolerance
                AdaptGeneExpression(genotype, GeneType.EnvironmentalTolerance, 0.1f);
                AdaptGeneExpression(genotype, GeneType.StemThickness, 0.05f); // Thicker stems for cold
            }
            
            // Adapt to humidity
            if (conditions.Humidity < 40f)
            {
                // Increase drought tolerance
                AdaptGeneExpression(genotype, GeneType.DroughtTolerance, 0.1f);
                AdaptGeneExpression(genotype, GeneType.LeafWaxiness, 0.05f); // More waxy leaves
            }
            
            // Adapt to light intensity
            if (conditions.LightIntensity > 1000f)
            {
                // Adapt to high light
                AdaptGeneExpression(genotype, GeneType.LightTolerance, 0.1f);
                AdaptGeneExpression(genotype, GeneType.TrichromeProduction, 0.05f); // More trichomes for protection
            }
            
            LogInfo($"Applied adaptive changes to genotype {genotype.GenotypeId}");
        }
        
        private void AdaptGeneExpression(CannabisGenotype genotype, GeneType geneType, float change)
        {
            var gene = _geneLibrary.GetGenesByType(geneType).FirstOrDefault();
            if (gene == null) return;
            
            if (genotype.Alleles.TryGetValue(gene.GeneId, out var alleles))
            {
                foreach (var allele in alleles)
                {
                    allele.Expression += change;
                    allele.Expression = Mathf.Clamp01(allele.Expression);
                }
            }
        }
        
        private void ProcessEpigeneticChanges()
        {
            // Implement epigenetic modifications based on environmental conditions
            foreach (var genotype in _genotypeDatabase.Values)
            {
                ProcessGenotypeEpigenetics(genotype);
            }
        }
        
        private void ProcessGenotypeEpigenetics(CannabisGenotype genotype)
        {
            // Epigenetic changes affect gene expression without changing DNA sequence
            // These changes can be inherited and provide rapid adaptation
            
            if (genotype.EpigeneticModifications == null)
            {
                genotype.EpigeneticModifications = new Dictionary<string, float>();
            }
            
            // Apply environmental epigenetic modifications
            var currentConditions = GetCurrentEnvironmentalConditions();
            ApplyEpigeneticModifications(genotype, currentConditions);
        }
        
        private EnvironmentalConditions GetCurrentEnvironmentalConditions()
        {
            // Get global environmental conditions
            return new EnvironmentalConditions
            {
                Temperature = 24f,
                Humidity = 60f,
                LightIntensity = 800f,
                CO2Level = 400f
            };
        }
        
        private void ApplyEpigeneticModifications(CannabisGenotype genotype, EnvironmentalConditions conditions)
        {
            // Temperature stress triggers epigenetic changes
            if (conditions.Temperature > 30f || conditions.Temperature < 18f)
            {
                var stressGenes = _geneLibrary.GetGenesByType(GeneType.StressResponse);
                foreach (var gene in stressGenes)
                {
                    var modificationKey = $"temp_stress_{gene.GeneId}";
                    genotype.EpigeneticModifications[modificationKey] = 
                        genotype.EpigeneticModifications.GetValueOrDefault(modificationKey, 0f) + 0.01f;
                }
            }
            
            // Light stress triggers protective responses
            if (conditions.LightIntensity > 1200f)
            {
                var protectionGenes = _geneLibrary.GetGenesByType(GeneType.TrichromeProduction);
                foreach (var gene in protectionGenes)
                {
                    var modificationKey = $"light_protection_{gene.GeneId}";
                    genotype.EpigeneticModifications[modificationKey] = 
                        genotype.EpigeneticModifications.GetValueOrDefault(modificationKey, 0f) + 0.005f;
                }
            }
        }
        
        #endregion
        
        #region System Updates and Analytics
        
        private void UpdateGeneticSystems()
        {
            _geneticProcessor?.Update();
            _breedingSimulator?.Update();
            _phenotypeEngine?.Update();
            _adaptationManager?.Update();
            _diversityTracker?.Update();
            _geneticsAnalytics?.Update();
        }
        
        private void UpdateGeneticDiversity()
        {
            _diversityTracker.CalculateDiversityMetrics(_genotypeDatabase.Values.ToList());
        }
        
        private void ProcessBreedingQueue()
        {
            // Process any queued breeding operations
            _breedingSimulator.ProcessBreedingQueue();
        }
        
        private void UpdatePerformanceMetrics()
        {
            _performanceMetrics.TotalGenotypes = _genotypeDatabase.Count;
            _performanceMetrics.TotalBreedingRecords = _breedingHistory.Count;
            _performanceMetrics.AverageGeneticDiversity = _diversityTracker.CalculateOverallDiversity();
            _performanceMetrics.UnlockedGenes = _unlockedGenes.Count;
            _performanceMetrics.DiscoveredTraits = _discoveredTraits.Count;
            _performanceMetrics.LastUpdate = DateTime.Now;
        }
        
        #endregion
        
        #region Event Handlers
        
        private void HandlePlantInstanceCreated(SpeedTreePlantInstance instance)
        {
            // Generate or assign genetics to new plant instance
            if (instance.GeneticData == null)
            {
                var genotype = GenerateGeneticVariation(instance.StrainAsset?.StrainId ?? "default");
                if (genotype != null)
                {
                    ApplyGeneticsToSpeedTree(instance, genotype);
                }
            }
        }
        
        private void HandleResearchCompleted(string researchId)
        {
            // Unlock new genes and breeding techniques based on research
            var unlockedGenes = GetGenesUnlockedByResearch(researchId);
            foreach (var geneId in unlockedGenes)
            {
                _unlockedGenes.Add(geneId);
                OnGeneUnlocked?.Invoke(geneId);
            }
        }
        
        private List<string> GetGenesUnlockedByResearch(string researchId)
        {
            // Map research projects to genetic unlocks
            return researchId switch
            {
                "advanced_genetics" => new List<string> { "thc_synthesis", "cbd_synthesis", "terpene_complex" },
                "breeding_techniques" => new List<string> { "flowering_time", "yield_optimization" },
                "plant_physiology" => new List<string> { "stress_tolerance", "environmental_adaptation" },
                _ => new List<string>()
            };
        }
        
        #endregion
        
        #region Public Interface
        
        public CannabisGenotype GetGenotype(string genotypeId)
        {
            return _genotypeDatabase.TryGetValue(genotypeId, out var genotype) ? genotype : null;
        }
        
        public List<CannabisGenotype> GetAllGenotypes()
        {
            return _genotypeDatabase.Values.ToList();
        }
        
        public List<CannabisGenotype> GetGenotypesByStrain(string strainId)
        {
            return _genotypeDatabase.Values.Where(g => g.StrainId == strainId).ToList();
        }
        
        public BreedingRecord GetBreedingRecord(string breedingId)
        {
            return _breedingHistory.TryGetValue(breedingId, out var record) ? record : null;
        }
        
        public List<BreedingRecord> GetBreedingHistory()
        {
            return _breedingHistory.Values.OrderByDescending(r => r.BreedingDate).ToList();
        }
        
        public GeneticLineage GetGeneticLineage(string genotypeId)
        {
            return _lineageTracker.TryGetValue(genotypeId, out var lineage) ? lineage : null;
        }
        
        public List<string> GetAvailableBreedingMethods()
        {
            var methods = new List<string>();
            
            if (_enableAdvancedBreeding)
            {
                methods.AddRange(new[] { "StandardCross", "Backcross", "LineBreeding", "Outbreeding" });
            }
            else
            {
                methods.Add("StandardCross");
            }
            
            return methods;
        }
        
        public bool CanPerformBreeding(string parent1Id, string parent2Id)
        {
            return _genotypeDatabase.ContainsKey(parent1Id) && 
                   _genotypeDatabase.ContainsKey(parent2Id) &&
                   parent1Id != parent2Id;
        }
        
        public GeneticsSystemReport GetSystemReport()
        {
            return new GeneticsSystemReport
            {
                PerformanceMetrics = _performanceMetrics,
                TotalGenotypes = _genotypeDatabase.Count,
                TotalBreedingRecords = _breedingHistory.Count,
                GeneticDiversity = _diversityTracker.CalculateOverallDiversity(),
                UnlockedGenes = new List<string>(_unlockedGenes),
                DiscoveredTraits = new List<string>(_discoveredTraits),
                ActiveAdaptations = _adaptationData.Count,
                SystemStatus = new Dictionary<string, bool>
                {
                    ["MendelianInheritance"] = _enableMendelianInheritance,
                    ["PolygeneticTraits"] = _enablePolygeneticTraits,
                    ["Mutations"] = _enableMutations,
                    ["Epigenetics"] = _enableEpigenetics,
                    ["EnvironmentalAdaptation"] = _enableEnvironmentalAdaptation,
                    ["AdvancedBreeding"] = _enableAdvancedBreeding
                }
            };
        }
        
        #endregion
        
        protected override void OnManagerShutdown()
        {
            // Stop all coroutines
            StopAllCoroutines();
            CancelInvoke();
            
            // Cleanup systems
            _geneticProcessor?.Cleanup();
            _breedingSimulator?.Cleanup();
            _phenotypeEngine?.Cleanup();
            _adaptationManager?.Cleanup();
            _diversityTracker?.Cleanup();
            _geneticsAnalytics?.Cleanup();
            
            // Disconnect events
            DisconnectSystemEvents();
            
            LogInfo("Cannabis Genetics Engine shutdown complete");
        }
        
        private void DisconnectSystemEvents()
        {
            if (_speedTreeManager != null)
            {
                _speedTreeManager.OnPlantInstanceCreated -= HandlePlantInstanceCreated;
            }
            
            if (_progressionManager != null)
            {
                _progressionManager.OnResearchCompleted -= HandleResearchCompleted;
            }
        }
    }
}