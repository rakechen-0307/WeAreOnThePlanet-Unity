# We Are On The Planet

## Introduction

Welcome to **We Are On The Planet**. In this project, everyone has one unique planet. You can chat with others, visit friends, finish tasks, build your own NFTs, and even transact your NFTs to gain some profits!
![image](https://hackmd.io/_uploads/rkMKlNF5T.png)

## Motive

As blockchain technology continues to progress, the widespread adoption of blockchain services appears increasingly likely in the future. However, for users unfamiliar with blockchain technology, navigating complex services can be challenging. Therefore, our goal is to establish a metaverse that simplifies and enhances the user experience, providing an accessible and entertaining platform for exploring diverse blockchain services.

## How to Use Our Application

### Requirements

- Metamask Wallet(Chrome browser) installed
- One computer should start as host for multiplayer gaming(At the current demo stage, facilitating transfers involves manual intervention by the host, who interacts with the Metamask interface on behalf of clients. While we acknowledge the potential for future automation of this process, it remains a manual task for hosts during this stage. The host only needs to click OK when ever any metamask interfave pops up.)

### Run The Game

- Host : run `./Application/Host/WeAreOnThePlanet.exe`
- Client : run `./Application/Client/WeAreOnThePlanet.exe`

## Feature

### Start Menu

Embark on your thrilling journey in our metaverse by either creating a new account with your email or logging in to an existing one.
![image](https://hackmd.io/_uploads/B19WAQY9a.png)

### Main Planet

Upon logging in, users will find themselves on the central hub known as the main planet. Here, a multitude of destinations and functionalities await, offering players an array of exploration and interaction opportunities.
![image](https://hackmd.io/_uploads/rkwSdHc5p.png)

#### Places

- Lobby: The initial landing point for users upon arrival at the planet.
- NFT Tree: A unique tree cultivating the fruits of NFTs, allowing players to showcase their personal NFT collections at their discretion. Moreover, the tree hole located at the base of the tree serves as the entrance to the NFT Workshop. Here, you can seamlessly explore, edit, and even craft your own NFTs using the innovative building system integrated into our game.
  ![image](https://hackmd.io/_uploads/H17CDBc96.png)
- Garden: An interactive space where players can cultivate distinctive plants. The more players engage with the available functions, the better their plants will flourish, ultimately blooming into a captivating display. Additionally, friends can visit and water plants periodically, fostering a collaborative environment to benefit both players in gaining experience.
  ![image](https://hackmd.io/_uploads/ByUW_r9ca.png)
- Meeting Square: The designated venue for NFT auctions, providing a space where players can convene with friends for enjoyable gatherings and bidding events.
  ![image](https://hackmd.io/_uploads/B1gu_SqqT.png)

#### UI

- Social Hub: Simply press the number 1 on your keyboard to access a menu where you can forge new connections, engage in text and voice conversations with friends, and embark on journeys to explore their planets. This menu is also your gateway to discovering NFT dealers for a unique trading experience.
  ![image](https://hackmd.io/_uploads/Hy4jOB5qT.png)
- Achievements: By pressing the number 2 on your keyboard, you can effortlessly review the accomplishments you've attained or are currently pursuing in your journey.
  ![image](https://hackmd.io/_uploads/rygc3dB59p.png)

### NFT Workshop

#### Build New NFT

Unleash your creativity by constructing your personalized NFT within a 10x10x10 cube, using an array of vibrant blocks at your disposal in the workshop. The possibilities are limitless, allowing you to shape and design your NFT with imaginative freedom.
![image](https://hackmd.io/_uploads/HyPROBqqa.png)

#### View Owned NFT

Delve into the intricacies of your NFT by accessing detailed information and make informed decisions about displaying it on the NFT tree. Moreover, leverage the building function to edit and refine your NFT, giving you full control over its unique characteristics.
![image](https://hackmd.io/_uploads/HJretScc6.png)

### NFT Dealer

The NFT dealer is your go-to for managing your NFTs with a range of services:

- Mint NFT: Transform your creations from the NFT workshop into official NFTs.
- Transfer NFT: Share the joy by gifting ownership of your NFTs to your friends, allowing them to cherish and enjoy your creations.
- Launch Auction: Initiate NFT auctions to sell your exceptional creations at a preferred price.
- Attend Auction: Participate in NFT auctions to bid and acquire the ones that captivate your interest.

![image](https://hackmd.io/_uploads/Hy5-YHq96.png)

### NFT Auction

Engage in thrilling auctions to obtain the NFTs that captivate your interest.
![image](https://hackmd.io/_uploads/H1JJRBqqp.png)

## Approach

### **Communication Structure**

![planetStructure.drawio (1)](https://hackmd.io/_uploads/SkvLYQK9a.png)

### **Unity Frontend**

We opted for Unity as our frontend development platform due to its extensive capabilities. Unity empowers us to use code for manipulating the behavior of 3D objects, constructing interactive UI elements, and designing intricate shading for the metaverse. Its versatile feature set provides a robust foundation for creating a dynamic and engaging user experience.
Furthermore, Unity offers a wealth of comprehensive documentation, enabling us to swiftly grasp its functionalities within the constraints of a hackathon's limited timeframe. This accessibility and support make Unity an ideal choice for efficient development and quick learning during the hackathon.

### Main Planet

#### Gravity System

The planet functions as a gravitational center, drawing all objects with mass towards its surface and maintaining them grounded. Additionally, it aligns the normal vector of these objects with the normal of the planet, ensuring a harmonized orientation.

### NFT Workshop

#### Placement System

To facilitate precise block placement within a 10x10x10 grid, we implemented Unity's raycasting system to determine the mouse click location. This system ensures that the placing position snaps to the grid, enabling players to create structured arrangements.
By storing the position and color data of the placed blocks, we can easily recreate the appearance of the NFTs constructed by players. This data allows us to showcase their creations within the game environment and provides the flexibility for subsequent editing.

#### View NFT

Building upon the stored NFT data, we can dynamically recreate the NFTs and render them into images for display within the menu. This functionality allows players to visually preview and appreciate their NFT creations directly in the game interface.
![image](https://hackmd.io/_uploads/S1zrOQY56.png)![image](https://hackmd.io/_uploads/H12w_XYca.png)

### Multiplayer

We use Unity Relay Service to handle multiplayer gaming. As long as one computer starts as the host, everyone can join the metaverse anytime, anywhere.
In the Login page, player join the game by entering the join code provided by the host.
![Login1](https://hackmd.io/_uploads/S1L2RQKqT.png)

### Message/Voice Chat

As for chatting with people in the metaverse, we use Unity Vivox Service to make users communicating with others without obstacles.

#### Message Chat

For each friend of a player, we assign an unique channel for the player and the friend, so only the player and the specific friend can see the message in their chat room.

#### Voice Chat

For all the people in the same planet, we create an audio channel for people to talk in.

### Art

#### Modeling

We utilize the 3D modeling software Blender to craft the visual aesthetics of the planet, plants, and various assets within the metaverse.
![image](https://hackmd.io/_uploads/HkrEafK9a.png)

#### Shading

By leveraging the shadergraph system of Unity, we have implemented a toon shader to bestow our metaverse with a distinctive and stylized appearance.
![image](https://hackmd.io/_uploads/HkrlnfK9T.png)
To achieve the toonish aesthetic, we wrote custom HLSL code to extract diffuse, specular, shadow, and distance attenuation information from Unity's Universal Render Pipeline (URP). By applying a fixed-value gradient, we successfully attained the desired stylized appearance.

#### Grass Generation

Enhancing the planet's surface with lifelike grass, we leveraged Unity's use of triangles for mesh construction. Through calculations based on the barycentric coordinates of each triangle, we positioned the grasses. Their rotation was adjusted to align with the planet surface's normal, while introducing a single degree of freedom for rotation to introduce a randomness to the grass arrangement.
For the grass color, we employed ShaderGraph to blend the grass color with the ground color, ensuring a cohesive and natural appearance. This approach contributes to a more harmonized and visually pleasing integration of the grass with the surrounding environment.
![image](https://hackmd.io/_uploads/B15yRzYc6.png)
To optimize performance, we harnessed Unity's GPU instancing, efficiently rendering a substantial number of grass instances with just a single draw call. This approach significantly enhances the efficiency of rendering large quantities of grass, contributing to a smoother and more responsive experience.

### **Database**

### Realm .NET SDK

We use MongoDB to store the data of players. For connecting MongoDB in Unity, we use the Realm .NET SDK to store, write and update player's data in time.

![database](https://hackmd.io/_uploads/SJXoX4FqT.png)

### **Blockchain**

### Token

We have two types of token : **WATP-T** and **WATP-N**.

#### WATP-T

This is the fungible token which players use to pay everything (e.g buy others' NFT) in the metaverse. We use ERC865, which is the extension of ERC20(ARC20) to implement this token. With ERC865, players in the mataverse do not need to have TAREA to pay the gas fee. They can pay the gas fee by this WATP-T token.

The following picture shows how the ERC865 implement gasless transaction:
![1000002991](https://hackmd.io/_uploads/r1dviTt5a.jpg)

The service provider will approve the transaction by verifying if the sender has the same address as the address that is used to sign the message. After that, the transaction will be executed and a fee will be sent to the service provider.

#### WATP-N

This is the non-fungible token(NFT) in the metaverse. We implement this by ERC721(ARC721) with several custom function similar to ERC865 to handle gasless transaction. For each WATP-N token minted or transferred, 5 WATP-T tokens will be paid as the gas fee.
The WATP-N tokens will be shown as several colorful blocks in the metaverse, and players can launch or attend an auction for it.

### Chainsafe Unity SDK

To connect Unity with Metamask and blockchain, we use the Chainsafe Unity SDK. This SDK provides players and hosts a concise interface to sign various messages, such as transfering and minting NFTs, which links the in-game trading system.

## Future Work

Given the time constraints during the hackathon, certain ideas were left unexplored. We envision incorporating these unfinished concepts into future versions of our metaverse, allowing for continuous improvement and expansion of features in subsequent releases.

### UI Polishing

Despite time constraints, we aim to enhance the visual appeal of our UI, striving for a more polished and stylistically fitting appearance. While acknowledging the current limitations, our commitment is to iteratively refine the UI to achieve a more desirable and cohesive aesthetic.

### Mini Games

We aspire to incorporate mini-games into our metaverse, providing users with opportunities to earn special achievements and, most importantly, to enhance their overall enjoyment.

### Event Hosting

We planned to introduce the capability for hosting events, such as concerts, within our metaverse. This addition aims to foster increased interaction among users, creating a vibrant and socially engaging environment for everyone involved.

### LLM Chatbots

In our upcoming development phases, we plan to integrate our metaverse with LLM APIs to introduce chatbots. These chatbots will serve as guides, offering users valuable insights and knowledge about blockchain technology. Additionally, users will have the opportunity to engage in conversations with NPCs, enriching their overall experience within the metaverse.

## Dependencies

### Unity

Editor version 2022.3.16f1
https://unity.com/
Relay
https://unity.com/products/relay
Vivox
https://unity.com/products/vivox-voice-chat

### Blender

version 4.0
https://www.blender.org/

### MongoDB

https://www.mongodb.com/

### Chainsafe

version 2.1.0
https://chainsafe.io/
