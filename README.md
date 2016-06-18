# gameBITS-Framework
gameBITS Framework: A framework for procedural content generation for digital games

The framework takes the following assumptions:
- The existence of a hierarchical organizational node system to manage the generated artifacts.
- The ability to generate content synchronously or asynchronously.
- The existence of a LOD system based on a set of simple rules.
- The distinction between concrete and abstract artifacts.
- The characterization of artifacts through a components system.
- The complete separation between characterization and generation of artifacts.

![alt tag](gameBITS%20Framework%20-%20Class%20Diagram.png)

The gameBits Framework architecture has fifteen different classes that compose the framework. 
- The class gBManager is responsible for centralizing the management of generation requests, it also holds the root artifact of the artifact hierarchy, grants registration and access to tools, as well as providing integration with the update cycle of the game engine and transmitting to each artifact the internal update order of each and its LOD systems.
- The gBObject class characterizes the interface that defines an object within the context generation system.
- gBTool defines an interface for classes that provide specific services, such as access to the game engine features, for example.
- gBSensor is a specialized class tool that aims to define a capture interface of any state of the virtual environment or performance data, providing access to this information mainly by the LOD system.
- gBGenerator is a specialized tool class that specifies an interface for accessing the generation of a particular resource, targeting itself to isolate the complexity of the same generation process.
- gBRequest is the class responsible for mediating requests between specific devices and generators synchronously or asynchronously.
- The class gBResource defines the basis of a resource generated procedurally, and is the one who contains the seed that determines the outcome generated.
- gBParameter is the class that features a parameter of an artifact, that is, it is what defines the characteristics of a particular artifact.
- gBArtifact describes a generic artifact, and is characterized by a collection of parameters.
- gBAbstract is the class definition of abstract artifacts, in general it characterizes resources such as meshes and textures, for example.
- The gBHelper class defines auxiliary structures for storage and manipulation of data, in general used to handle data not manipulated by the game engine.
- The gBConcrete class characterizes what is defined as concrete artifact, it consists of abstract artifacts, characterization parameters, may have a LOD control system, and is the class that characterizes the concept of node, it may be linked to a parent node and several child nodes.
- The gBLODSystem class defines an interface for LOD systems. The gBLODRuleSet class features a LOD system based on a Discrete LOD system, which manages the rules that define the presentation behavior of an artifact.
- The gBLODRule class describes a level of detail modification rule of an artifact, being able to dynamically change characteristics of this to suit a particular situation detected by sensors.
