# Event Service for Analytics (Test Project)

This repository contains a test implementation of a small service designed to handle event tracking for an analytics server. The service is developed to run on Android and WebGL platforms using Unity 2021 LTS.

## Test Project Overview

This test task aims to develop a service that receives and sends events to an analytics server. It demonstrates the ability to handle event tracking efficiently, ensuring reliable delivery even in challenging network conditions.

## Test Requirements

- **Platforms:** Android and WebGL.
- **Events:** Examples include level start, reward received, and coin spent.
- **Event Format:** Each event is an object with the following fields:
  - `type`: String representing the event type.
  - `data`: String containing event data.
- **Server Communication:**
  - Events are sent in batches via a POST request in JSON format.
  - The server endpoint is set by an external parameter `serverUrl`.
- **Cooldown Mechanism:**
  - A cooldown (`cooldownBeforeSend`) of 1-3 seconds is used to accumulate events and reduce server requests.
  - The first event starts the cooldown; accumulated events are sent after the cooldown.
- **Guaranteed Delivery:** Events must be delivered to the server, confirmed by a 200 OK response.
- **Offline Support:** Undelivered events are stored and resent on the next application launch.
- **Development Tools:** Unity 2021 LTS, with optional additional libraries.

## Implementation

The core logic is encapsulated in a `MonoBehaviour` class, `EventService`, with the following method: `TrackEvent(string type, string data)`

## Usage

1. **Clone the Repository:** Download and open the project in Unity 2021 LTS.
2. **Integrate the Service:** Instantiate and use the `EventService` within your game logic.
3. **Configure Server URL:** Set the `serverUrl` parameter for event posting.

## Key Features

- **Batch Event Sending:** Reduces server load by sending events in batches after a cooldown.
- **Reliability:** Ensures events are delivered, even after app restarts or network interruptions.
- **Minimal Setup:** Focused on core service logic without unnecessary complexity.

## Conclusion

This test project showcases the ability to implement a robust event tracking service within Unity, adhering to given requirements and demonstrating efficient handling of network communications.
