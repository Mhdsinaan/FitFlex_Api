🏋 FitFlex – Gym Management & Member Engagement System
Project Overview

FitFlex is a web-based gym management platform designed to help gym owners, trainers, and members manage memberships, bookings, workouts, diet plans, payments, and real-time communication.

This project uses Clean Architecture and Entity Framework Core (Code-First) in .NET, with a React frontend (optional) or any client app.

Features
Admin / Gym Owner

Manage users: members and trainers

Approve memberships and trainer profiles

Track payments and bookings

Generate reports for member progress

Member

Register and login

View and subscribe to membership plans

Book sessions with trainers (personal or group)

Track workout and diet progress

Chat with trainers or join group chats

Trainer

Manage availability and profile

View member bookings

Track member progress

Chat with members individually or in groups

Shared / Core

JWT-based authentication

Role-based access control (Admin, Trainer, Member)

Real-time chat via SignalR

EF Core Code-First database with SQL Server

Clean Architecture (Domain → Application → Infrastructure → API)

Technologies Used

Backend: .NET 8 / ASP.NET Core Web API

Database: SQL Server + Entity Framework Core (Code-First)

Frontend (optional): React.js / Tailwind CSS

Real-time Chat: SignalR

Authentication: JWT

Other: AutoMapper, FluentValidation
