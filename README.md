# User Microservice

A lightweight microservice built with .NET 8 to manage user registration, email confirmation, login (local and social), and profile management as part of a larger microservices architecture.

## Overview

The **User Microservice** is responsible for:
- User registration with email/password.
- Email confirmation flow.
- Secure login with JWT token generation.
- User profile management.
- (Future work) Social logins (e.g., Google, GitHub) and refresh token issuance.

This microservice is built using a **Clean Architecture** approach to promote separation of concerns, testability, and scalability.

## Architecture

The project follows a layered/clean architecture:
UserService/
- Application/ <-- Business logic, use cases, DTOs, interfaces.
-  Domain/ <-- Core entities (User, etc.).
- Infrastructure/ <-- Data access (EF Core), email services, JWT generation.
- WebAPI/ <-- Controllers, configuration, entry point.
- Shared/ <-- Common utilities, error handling, DTOs.
