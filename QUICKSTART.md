# ?? Quick Start Guide

Get your Residence CRUD API up and running in 5 minutes!

---

## 1?? Prerequisites

- ? .NET 8 SDK (installed)
- ? SQL Server LocalDB (installed)
- ? Visual Studio / VS Code (optional)

---

## 2?? Build the Project

```bash
cd C:\Users\RAHMANI Fares\source\repos\Residence-app

dotnet build
```

**Expected Output:**
```
Build succeeded.
```

---

## 3?? Run the Application

```bash
dotnet run --project residence.api
```

**Expected Output:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7000
```

---

## 4?? Access the API

### Option A: Swagger UI (Recommended)
```
https://localhost:7000/swagger
```

1. Open link in browser
2. All endpoints are listed
3. Click "Try it out" on any endpoint
4. Click "Execute"
5. See response

### Option B: Command Line (curl)
```bash
# Get all residences
curl https://localhost:7000/api/residences -k

# Get residences by ID
curl https://localhost:7000/api/residences/1 -k
```

### Option C: Postman
1. Create new request
2. Set method to GET
3. URL: `https://localhost:7000/api/residences`
4. Send request

---

## 5?? Test Create Endpoint

### Using Swagger UI:
1. Go to `https://localhost:7000/swagger`
2. Find `POST /api/residences`
3. Click "Try it out"
4. Enter this JSON in request body:
```json
{
  "name": "My Home",
  "address": "123 Main Street",
  "city": "New York",
  "state": "NY",
  "zipCode": "10001",
  "numberOfRooms": 3,
  "price": 300000,
  "description": "Beautiful home"
}
```
5. Click "Execute"
6. See 201 Created response

### Using curl:
```bash
curl -X POST https://localhost:7000/api/residences \
  -H "Content-Type: application/json" \
  -d "{\"name\":\"My Home\",\"address\":\"123 Main St\",\"city\":\"NYC\",\"state\":\"NY\",\"zipCode\":\"10001\",\"numberOfRooms\":3,\"price\":300000,\"description\":\"Nice home\"}" \
  -k
```

---

## ?? All Available Endpoints

| Method | Endpoint | Purpose |
|--------|----------|---------|
| GET | `/api/residences` | Get all residences |
| GET | `/api/residences/1` | Get residences by ID (replace 1 with any ID) |
| POST | `/api/residences` | Create new residence |
| PUT | `/api/residences/1` | Update residence (replace 1 with ID) |
| DELETE | `/api/residences/1` | Delete residence (replace 1 with ID) |

---

## ?? Quick Test Suite

### Test 1: Get All
```bash
curl https://localhost:7000/api/residences -k
```
? Expected: Array of residences (200 OK)

### Test 2: Get One
```bash
curl https://localhost:7000/api/residences/1 -k
```
? Expected: Single residence object (200 OK)

### Test 3: Get Non-Existent
```bash
curl https://localhost:7000/api/residences/999 -k
```
? Expected: Empty response (404 Not Found)

### Test 4: Create Valid
```bash
curl -X POST https://localhost:7000/api/residences \
  -H "Content-Type: application/json" \
  -d '{"name":"Test","address":"123","city":"NYC","state":"NY","zipCode":"10001","numberOfRooms":2,"price":100000}' \
  -k
```
? Expected: New residence with ID (201 Created)

### Test 5: Create Invalid (Missing Name)
```bash
curl -X POST https://localhost:7000/api/residences \
  -H "Content-Type: application/json" \
  -d '{"address":"123","city":"NYC","state":"NY","zipCode":"10001","numberOfRooms":2,"price":100000}' \
  -k
```
? Expected: Error message (400 Bad Request)

### Test 6: Update
```bash
curl -X PUT https://localhost:7000/api/residences/1 \
  -H "Content-Type: application/json" \
  -d '{"name":"Updated","address":"456","city":"Boston","state":"MA","zipCode":"02101","numberOfRooms":3,"price":250000}' \
  -k
```
? Expected: Updated residence (200 OK)

### Test 7: Delete
```bash
curl -X DELETE https://localhost:7000/api/residences/1 -k
```
? Expected: Empty response (204 No Content)

---

## ??? Troubleshooting

### Issue: "Connection refused"
**Solution:** Make sure API is running: `dotnet run --project residence.api`

### Issue: "SSL Certificate Error"
**Solution:** Add `-k` flag to curl or disable certificate verification in Postman

### Issue: "Database not found"
**Solution:** 
1. Ensure LocalDB is installed
2. Connection string in `appsettings.json` is correct
3. Delete `ResidenceDb.mdf` file if it exists and restart API

### Issue: "Port 7000 already in use"
**Solution:** 
1. Stop the previous instance
2. Or run on different port: `dotnet run --project residence.api -- --urls "https://localhost:7001"`

---

## ?? Documentation Files

| File | Purpose |
|------|---------|
| README.md | Getting started |
| ARCHITECTURE.md | Architecture patterns |
| QUICK_REFERENCE.md | Developer reference |
| TESTING.md | Testing guide |
| DIAGRAMS.md | Architecture diagrams |
| PROJECT_SUMMARY.md | Complete summary |

---

## ?? Key Concepts

### IEntityTypeConfiguration
Separate configuration from DbContext for cleaner code:
```csharp
// Instead of inline configuration in DbContext
builder.ApplyConfiguration(new ResidenceConfiguration());
```

### MapEndpoints Pattern
Organize endpoints in static class:
```csharp
// Instead of inline endpoints in Program.cs
app.MapResidenceEndpoints();
```

### Clean Architecture
```
API Layer (Endpoints)
?
Application Layer (Services)
?
Domain Layer (Entities)
?
Infrastructure Layer (Data Access)
?
SQL Server Database
```

---

## ?? Database

**Connection String:**
```
Server=(localdb)\\mssqllocaldb;Database=ResidenceDb;Trusted_Connection=true;
```

**Table: dbo.Residences**
- Automatically created on first run
- Includes all columns with constraints
- Timestamps auto-managed by database

---

## ?? What's Next?

1. ? Test all endpoints
2. ? Read ARCHITECTURE.md
3. ? Run test cases from TESTING.md
4. ? Add more entities following the pattern
5. ? Implement unit tests
6. ? Add logging
7. ? Deploy to production

---

## ?? Pro Tips

1. **Bookmark Swagger UI**: `https://localhost:7000/swagger`
2. **Use Postman**: Save requests for re-use
3. **Check Logs**: Watch console output for errors
4. **Read ARCHITECTURE.md**: Understand the pattern
5. **Use QUICK_REFERENCE.md**: Copy-paste code for new features

---

## ?? Common Times

- **Build**: ~5 seconds
- **Run**: ~2 seconds to listening
- **First Request**: ~1 second (cold start)
- **Subsequent Requests**: <100ms

---

## ?? Common Commands

### Build
```bash
dotnet build
```

### Build and Run
```bash
dotnet run --project residence.api
```

### Run Tests (when available)
```bash
dotnet test
```

### Check Code Issues
```bash
dotnet build --verbosity detailed
```

### Clean Build
```bash
dotnet clean
dotnet build
```

---

## ? Architecture at a Glance

```
???????????????????????????????????????
?         Swagger UI                  ?
?    https://localhost:7000/swagger   ?
???????????????????????????????????????
                 ?
???????????????????????????????????????
?     ResidenceEndpoints.cs           ?
?    (Static MapEndpoints methods)    ?
???????????????????????????????????????
                 ?
???????????????????????????????????????
?     ResidenceService                ?
?    (Business Logic & DTO Mapping)   ?
???????????????????????????????????????
                 ?
???????????????????????????????????????
?     ResidenceRepository             ?
?    (Data Access with EF Core)       ?
???????????????????????????????????????
                 ?
???????????????????????????????????????
?     ApplicationDbContext            ?
?    (Uses ResidenceConfiguration)    ?
???????????????????????????????????????
                 ?
???????????????????????????????????????
?    SQL Server LocalDB               ?
?     dbo.Residences Table            ?
???????????????????????????????????????
```

---

## ?? You're All Set!

Your Residence CRUD API is ready to use!

### Next Step
Open Swagger UI:
```
https://localhost:7000/swagger
```

Then click "Try it out" on any endpoint!

---

**Happy Coding! ??**

For more details, see:
- ARCHITECTURE.md - Deep dive into patterns
- QUICK_REFERENCE.md - Copy-paste templates
- TESTING.md - Complete testing guide
