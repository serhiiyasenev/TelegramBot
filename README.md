# TelegramBot

> A clean .NET Telegram bot implementation with ready-to-use command handlers.  
Solution file: **TgBot.sln**

---

## 🔥 Features
- Handle text commands (e.g., `/start`, `/help`).
- Separation of business logic and infrastructure.
- Support for **Dependency Injection**, **logging**, and **configuration**.
- Ready-to-use templates for **handlers**, **middlewares**, and **services**.
- Secure **Bot Token** handling via secrets/environment variables.
- Easy to run locally or inside **Docker**.

---

## 📦 Tech Stack
- **.NET** (Console App / Worker)
- **Telegram Bot API** (via official or popular .NET clients)
- **Microsoft.Extensions** (Logging, Configuration, Hosting)

---

## 🚀 Getting Started

### 1) Prerequisites
1. Install **.NET SDK** (7 or 8).
2. Get a **Telegram Bot Token** from [@BotFather](https://t.me/BotFather).

### 2) Configuration
Recommended way: **User Secrets** (local) or **Environment Variables** (CI/CD, Docker).

**Using User Secrets (local):**
```bash
cd TgBot
dotnet user-secrets init
dotnet user-secrets set "Telegram:BotToken" "123456:ABC..."
```

**Using Environment Variables:**
```bash
# Windows (PowerShell)
$env:Telegram__BotToken="123456:ABC..."

# Linux / macOS (bash)
export Telegram__BotToken="123456:ABC..."
```

🐳 **Run in Docker**
```dockerfile
# 1) build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore TgBot/TgBot.csproj \
 && dotnet publish TgBot/TgBot.csproj -c Release -o /app

# 2) runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=build /app .
# Pass token via env var
# ENV Telegram__BotToken=123456:ABC...
ENTRYPOINT ["dotnet", "TgBot.dll"]
```
**Build & Run:**
```bash
docker build -t telegram-bot .
docker run --rm -e Telegram__BotToken="123456:ABC..." telegram-bot
```

### 🧷 Useful Links
- [BotFather — create token](https://t.me/BotFather)  
- [Telegram Bot API documentation](https://core.telegram.org/bots/api)  
- [.NET SDK downloads](https://dotnet.microsoft.com/download)
