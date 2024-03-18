This demo contains:
- GasSensorTwin.json: a Digital Twin (DT) model definition file
- SimulatedGasSensor.json: a Simulation model definition file 
- 2 CSV files (to initialize instances for each model in the context of a simulation)

The DT model defines state properties and message rules to handle messages received from gas sensors. In essence, the rules update state properties from received messages, and handle extreme conditions (either spike values or prolonged increase) by setting an Alarm and sending a shut-off message back to the source.

The SimulatedGasSensor simulation file is used to model real sensors sending messages to the Digital Twins. By setting the Behavior property of the simulated instances (Normal, Spike, or SlowRise), you can define the behavior of the simulated sensors.

With each iteration of the simulation, a counter is increased. Once the counter reaches the CounterBeforeBehavior value, the behavior starts. In case of a Spike, the PPMReading is set to 225 immediately. In case of a SlowRise, the PPMReading increases by 5 with each iteration.

The 2 CSV files offer a basic simulation with 4 sensors, 2 of which will have a normal behavior (maintaining PPM readings within acceptable boundaries) and one sensor for each of the other behavior types (Spike and SlowRise).