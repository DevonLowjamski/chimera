# Project Chimera Development Roadmap

**Guiding Philosophy:** From Façade to Function. Move from well-designed but empty systems to a functional, demonstrable core. Build depth layer by layer.

---

### **Phase 0: Foundation & Pre-Production (1-2 Weeks)**

**Goal:** Establish tools and standards for efficient development and debugging.

*   **Milestone 0.1: Establish a Robust Testing Framework.**
    *   **Task 0.1.1:** Create a dedicated "Test Harness" scene in Unity.
    *   **Task 0.1.2:** Implement a basic test runner script for programmatic simulation tests.
    *   **Task 0.1.3:** Write a first unit test for `PlantInstanceSO.InitializePlant()` to verify the framework.

*   **Milestone 0.2: Develop the "Chimera Debug Inspector."**
    *   **Task 0.2.1:** Create a new runtime UI panel, accessible via a hotkey.
    *   **Task 0.2.2:** The panel must display real-time data for a selected plant: ID, Strain, Age, Health, Stress, Water, Nutrients, the complete expressed phenotype, and the complete genotype.
    *   **Task 0.2.3:** The inspector must also display the current `EnvironmentalConditions` of the plant's zone.

*   **Milestone 0.3: Formalize Version Control and Task Management.**
    *   **Task 0.3.1:** Enforce a strict Git branching strategy (e.g., GitFlow).
    *   **Task 0.3.2:** Break down this roadmap into tickets in a project management tool.

---

### **Phase 1: The Core Simulation - The "G" in GxE (3-4 Weeks)**

**Goal:** Make a single plant grow and visibly change based on its unique genes in a static, optimal environment.

*   **Milestone 1.1: Implement Functional Genetic Inheritance.**
    *   **Task 1.1.1:** Refactor `BreedingSimulator.cs`, replacing placeholder logic.
    *   **Task 1.1.2:** Implement Mendelian Inheritance: offspring must receive one random allele from each parent for each gene.
    *   **Task 1.1.3:** Unit Test: Verify Mendelian ratios (`AA` x `aa` -> 100% `Aa`; `Aa` x `Aa` -> ~25% `AA`, 50% `Aa`, 25% `aa`).

*   **Milestone 1.2: Implement Functional Trait Expression.**
    *   **Task 1.2.1:** Refactor `TraitExpressionEngine.cs`, replacing placeholder logic.
    *   **Task 1.2.2:** Implement Dominance Logic to calculate a single trait value (e.g., `HeightMultiplier`) from a genotype.
    *   **Task 1.2.3:** Unit Test: Verify that different genotypes (`HH`, `Hh`, `hh`) produce the correct, predictable trait values.

*   **Milestone 1.3: Connect Genetics to Growth.**
    *   **Task 1.3.1:** Refactor `PlantInstanceSO.ProcessDailyGrowth`.
    *   **Task 1.3.2:** The method must now call the `TraitExpressionEngine` to get the plant's currently expressed phenotype.
    *   **Task 1.3.3:** The expressed phenotype (e.g., `HeightMultiplier`) must directly modify the plant's `_dailyGrowthRate`.
    *   **Task 1.3.4:** Integration Test: In the Test Harness, grow three seeds with different height genotypes (`HH`, `Hh`, `hh`) and assert that their final heights are correctly ordered.

**Phase 1 Deliverable:** A playable scene where genetically different seeds grow into visibly different plants.

---

### **Phase 2: The Interactive Environment - The "E" in GxE (2-3 Weeks)**

**Goal:** Make player actions and facility design have a direct, measurable impact on plant growth.

*   **Milestone 2.1: Implement Zone-Based Environmental Simulation.**
    *   **Task 2.1.1:** Refactor `CultivationManager` to manage a dictionary of `CultivationZone` objects, each with its own `EnvironmentalConditions`.
    *   **Task 2.1.2:** Assign plants to a zone and have them read from that zone's environment.
    *   **Task 2.1.3:** Implement a basic thermal model for Temperature in the `CultivationZone`.

*   **Milestone 2.2: Implement Player Interaction.**
    *   **Task 2.2.1:** Create a simple `Heater` equipment script with `TurnOn()`/`TurnOff()` methods.
    *   **Task 2.2.2:** When a `Heater` is on, the zone's temperature should drift towards a higher target.
    *   **Task 2.2.3:** Connect the zone's temperature to the `GrowthCalculationSO`'s `temperatureResponseCurve` to affect growth.
    *   **Task 2.2.4:** Create a UI button to control the heater and display the zone's temperature.

*   **Milestone 2.3: Verification.**
    *   **Task 2.3.1:** Integration Test: Grow two identical plants in separate zones, heat one zone, and assert that the final heights are different.

**Phase 2 Deliverable:** A playable scene where a player action (turning on a heater) impacts the environment and plant growth.

---

### **Phase 3: The Gameplay Loop - Closing the Circle (2-3 Weeks)**

**Goal:** Provide player motivation and a core economic loop.

*   **Milestone 3.1: Implement Harvesting and Economy.**
    *   **Task 3.1.1:** Implement the `Harvest()` function to calculate yield based on plant size and health.
    *   **Task 3.1.2:** Create a simple `PlayerWallet.cs`.
    *   **Task 3.1.3:** Create a "Market" UI to sell harvests and add currency to the wallet.

*   **Milestone 3.2: Implement Core Progression.**
    *   **Task 3.2.1:** Implement a cost system for purchasing seeds and equipment.
    *   **Task 3.2.2:** Gate one piece of equipment (e.g., a `GrowLight`) behind a currency requirement in a "Shop" UI.
    *   **Task 3.2.3:** Implement the `GrowLight`'s effect on the environmental simulation and growth calculation.

**Phase 3 Deliverable:** A complete, simple gameplay loop: Plant -> Grow -> Harvest -> Sell -> Upgrade -> Grow Better.

---

### **Phase 4 & Beyond: Expansion and Deepening**

*   **Genetics Expansion:** Implement Polygenic Traits, Pleiotropy, Epistasis, and the Genetics Lab UI.
*   **Environmental Expansion:** Implement Humidity, CO2, pH, Nutrients, and the corresponding equipment and automation systems.
*   **Gameplay Expansion:** Build out the NPC contract system, Skill Tree, Research system, and the detailed facility construction system.
*   **UI/UX Polish:** Flesh out all UI panels and connect them to the functional backend systems.
