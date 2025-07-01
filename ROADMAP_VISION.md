# Project Chimera: Total Vision Development Roadmap

**Guiding Philosophy:** From a meticulously researched vision to a deeply simulated, living world. We will build a genre-defining simulation by layering complex, interconnected systems upon a robust, scientifically-grounded core. Every step is a move towards creating the most authentic and engaging cultivation and management game ever made.

---

## **Phase 0: Foundation & Tooling (The Bedrock)**

**Goal:** Establish a professional-grade development environment, robust testing frameworks, and the core data structures that will underpin all future systems. This phase is about building the clean room before the complex work begins.

*   **Milestone 0.1: Advanced Project Setup & Version Control**
    *   Task: Formalize a strict GitFlow or similar branching strategy.
    *   Task: Configure Assembly Definitions (.asmdef) for modularity (e.g., Core, Genetics, Environment, Simulation, UI, AI).
    *   Task: Implement project-wide coding standards enforcement with static analysis tools.

*   **Milestone 0.2: The "Chimera Test Harness"**
    *   Task: Create a dedicated Unity scene for isolated system testing.
    *   Task: Implement a programmatic test runner for running simulation scenarios without manual intervention (e.g., "Grow plant X in environment Y for Z days").
    *   Task: Write foundational unit tests for core data objects (e.g., Genotype, Phenotype, EnvironmentalConditions).

*   **Milestone 0.3: The "Chimera Debug Inspector" (Runtime Diagnostics UI)**
    *   Task: Create a runtime UI panel, accessible via hotkey.
    *   Task: Display real-time data for any selected plant, including its full genotype, expressed phenotype, health, stress, growth stage, and resource levels.
    *   Task: Display real-time data for any selected cultivation zone, showing all environmental parameters (temperature, humidity, CO2, light levels, etc.).
    *   Task: Add time control features (pause, 1x, 10x, 100x speed) to the debug inspector for rapid simulation testing.

*   **Milestone 0.4: Core Data Architecture (ScriptableObjects)**
    *   Task: Create the foundational ScriptableObject (SO) definitions for `GeneDefinitionSO`, `AlleleSO`, and `PlantSpeciesSO`. These will define the absolute genetic possibilities.
    *   Task: Create the `EnvironmentalParameterSO` to define different environmental variables (e.g., Temperature, Humidity) and their min/max safe ranges.
    *   Task: Create initial `EquipmentDataSO` for basic equipment types, defining their core attributes (cost, power usage, etc.).

---

## **Phase 1: The "G" in GxE (The Genetic Engine)**

**Goal:** Breathe life into the plants. Implement a sophisticated, multi-layered genetics system that goes far beyond simple Mendelian inheritance, making breeding a deep and rewarding core gameplay mechanic.

*   **Milestone 1.1: Foundational Trait Expression**
    *   Task: Implement the `TraitExpressionEngine` to calculate a plant's phenotype from its genotype.
    *   Task: Implement basic Dominant/Recessive logic for a single, visible trait (e.g., Leaf Color) to start.
    *   **Unit Test:** Verify that `AA`, `Aa`, and `aa` genotypes produce the correct, predictable phenotypes.

*   **Milestone 1.2: Advanced Genetic Inheritance**
    *   Task: Implement the `BreedingSimulator` for creating offspring.
    *   Task: Implement **Polygenic Inheritance**, where multiple genes (e.g., `YieldGene01`, `YieldGene02`) additively contribute to a single trait (e.g., MaxYield). Alleles will have effect sizes.
    *   Task: Implement **Pleiotropy**, where a single gene can influence multiple, seemingly unrelated traits.
    *   Task: Implement **Epistasis**, where the effect of one gene is modified by one or several other genes.
    *   **Unit Test:** Create complex breeding scenarios (e.g., `AABbCc` x `aaBBCc`) and verify that offspring genotypes and resulting phenotype value distributions match statistical expectations.

*   **Milestone 1.3: Genetic-Physiological Connection**
    *   Task: Refactor `PlantInstance.ProcessDailyGrowth` to be driven by the fully expressed phenotype.
    *   Task: Connect genetic traits like `HeightMultiplier`, `WaterUptakeRate`, and `NutrientEfficiency` directly to the plant's physiological simulation.
    *   **Integration Test:** In the Test Harness, grow three genetically distinct strains (e.g., one tall and thirsty, one short and efficient, one high-yield) and assert that their final size, resource consumption, and yield are significantly different and predictable.

*   **Milestone 1.4: The Genetics Lab UI (Initial Mockup)**
    *   Task: Design and implement a non-functional UI mockup for the Genetics Lab.
    *   Task: This UI should have placeholders for viewing a plant's full genome, comparing parent/offspring genotypes, and visualizing polygenic trait potentials. This provides a clear target for the functional implementation later.

---

## **Phase 2: The "E" in GxE (The Living Environment)**

**Goal:** Create a dynamic, physics-based environment that is as much a character as the plants themselves. The player doesn't just set numbers; they build and manage a complex, simulated ecosystem.

*   **Milestone 2.1: Zone-Based Environmental Simulation**
    *   Task: Implement `CultivationZone` objects, each with its own `EnvironmentalConditions` data.
    *   Task: Implement a robust thermal model: heat transfer between zones, heat generation from equipment (lights), and dissipation to the outside world.
    *   Task: Implement models for **Humidity** (transpiration from plants, dehumidifiers) and **CO2** (plant consumption, ventilation).
    *   Task: Implement a basic **Electrical Grid** system (`PowerSource`, `PowerConsumer` components) that equipment must connect to.

*   **Milestone 2.2: GxE Interaction & Plant Stress**
    *   Task: Implement `EnvironmentalResponseCurveSO` (using AnimationCurve) to define how a plant's genetics respond to specific environmental parameters.
    *   Task: A plant's `GrowthRate` is now a function of `GeneticPotential * GxEResponseModifier`.
    *   Task: Implement a **Stress Model**. When environmental conditions are outside a plant's genetically-defined optimal range, it accumulates stress, which negatively impacts health, growth, and can lead to disease susceptibility.
    *   **Integration Test:** Grow two identical plants in separate zones. Heat one zone beyond the plant's optimal temperature. Assert that the stressed plant has a lower final yield and health.

*   **Milestone 2.3: Substrate & Irrigation Systems**
    *   Task: Implement different growing substrates (e.g., `SoilSO`, `CocoCoirSO`, `HydroponicSystemSO`) with different properties (water retention, aeration, nutrient buffering).
    *   Task: Implement a physics-based **Irrigation System** with pipes, pumps, and reservoirs. Players must design and build plumbing networks.
    *   Task: Implement a `NutrientMixingSystem` where players can create custom fertilizer recipes from base chemical components (`Nitrogen`, `Phosphorus`, `Potassium`, etc.).
    *   Task: The plant's nutrient uptake is now dependent on the available nutrients in its specific substrate, as supplied by the irrigation system.

---

## **Phase 3: The Gameplay Loop & Economy**

**Goal:** Close the loop. Provide the player with motivation, progression, and a dynamic economic landscape to navigate. This phase turns the simulation into a *game*.

*   **Milestone 3.1: Harvest, Processing, and Quality**
    *   Task: Implement the `Harvest()` function, calculating yield based on the plant's final size, health, and genetic potential.
    *   Task: Implement post-harvest processing states: **Drying** and **Curing**. The conditions of the drying/curing room (temp/humidity) and duration will dramatically affect the final product's quality and value.
    *   Task: The final product has a detailed quality profile (e.g., Cannabinoid content, Terpene profile, visual appeal) derived from its genetics, growth conditions, and post-harvest handling.

*   **Milestone 3.2: The Dynamic Marketplace & NPCs**
    *   Task: Implement a basic `PlayerWallet` and a simple "sell" UI.
    *   Task: Implement an **NPC-driven Economy**. Create `NPCProfileSO`s for different buyers (e.g., a dispensary owner who wants high THC, a chef who wants specific terpenes).
    *   Task: Implement a **Contract System**. NPCs will generate dynamic contracts with specific quality requirements and rewarding premiums. This becomes the primary driver of player activity.
    *   Task: Market prices for bulk goods will fluctuate based on supply and demand.

*   **Milestone 3.3: Research & Progression**
    *   Task: Implement a **Skill Tree** system (`SkillNodeSO`). Players earn XP from successful harvests and contracts, which they can spend to unlock new skills (e.g., "Advanced Nutrient Mixing," "HVAC Efficiency Tuning," "Genetic Splicing").
    *   Task: Implement a **Research System**. Players can initiate research projects (`ResearchProjectSO`) to unlock new equipment, genetic markers, or advanced cultivation techniques.

*   **Milestone 3.4: Facility Construction**
    *   Task: Implement a basic grid-based construction system.
    *   Task: Players can now build new rooms (walls, doors) and place equipment.
    *   Task: The construction materials used will have physical properties (e.g., insulation), tying back into the environmental simulation.

---

## **Phase 4: The Living World & Endgame**

**Goal:** Transform the game from a mechanical simulation into a rich, living world filled with challenges, stories, and long-term goals.

*   **Milestone 4.1: Advanced Challenges**
    *   Task: Implement **Pests and Diseases**. Pests (`PestSO`) can appear and spread, while diseases (`DiseaseSO`) can arise from plant stress or unsanitary conditions. Players must react with sanitation protocols and integrated pest management.
    *   Task: Implement **Equipment Malfunctions and Degradation**. Equipment will require maintenance and can break down, creating dynamic challenges for the player.

*   **Milestone 4.2: Narrative & World-Building**
    *   Task: Implement a simple event system that can trigger narrative vignettes or world events (e.g., a heatwave, a new regulation, a visit from a cannabis celebrity).
    *   Task: Create a cast of recurring NPC characters who guide the player, offer unique, high-value contracts, and drive a light narrative arc.

*   **Milestone 4.3: Competition & Endgame**
    *   Task: Implement the **Cannabis Cup** event. Players can submit their best products to be judged against AI competitors on various criteria. Winning brings prestige, unique rewards, and unlocks new game tiers.
    *   Task: Implement a "Legacy" system, where players can eventually "retire" a facility and start a new one with bonuses based on their previous success, encouraging replayability.

*   **Milestone 4.4: Polishing & UX**
    *   Task: Overhaul all UI to be fully functional, visually appealing, and provide excellent data visualization (graphs, heatmaps).
    *   Task: Implement a comprehensive tutorial and in-game encyclopedia (`Chimera-pedia`) that explains the deep simulation mechanics to the player.
    *   Task: Full sound design pass, audio feedback for all systems.
    *   Task: Optimization pass for performance, especially on large, complex facilities.
