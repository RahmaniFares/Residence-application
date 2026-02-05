# API Testing Guide

## Running the Application

### Start the API
```bash
dotnet run --project residence.api
```

The API will be available at:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:7000`
- **Swagger UI**: `https://localhost:7000/swagger`

---

## Testing Endpoints with cURL

### Prerequisites
- cURL installed (Windows 10+ has it built-in)
- API running on `https://localhost:7000`
- Add `-k` flag to curl commands to ignore SSL certificate warnings (dev only)

---

## 1. GET ALL RESIDENCES

**Request:**
```bash
curl -X GET https://localhost:7000/api/residences -k
```

**Expected Response (200 OK):**
```json
[
  {
    "id": 1,
    "name": "Downtown Apartment",
    "address": "123 Main St",
    "city": "New York",
    "state": "NY",
    "zipCode": "10001",
    "numberOfRooms": 2,
    "price": 250000,
    "description": "Modern apartment",
    "createdAt": "2024-01-15T10:30:00Z",
    "updatedAt": "2024-01-15T10:30:00Z"
  },
  {
    "id": 2,
    "name": "Suburban House",
    "address": "456 Oak Ave",
    "city": "Boston",
    "state": "MA",
    "zipCode": "02101",
    "numberOfRooms": 4,
    "price": 450000,
    "description": "Spacious house",
    "createdAt": "2024-01-15T10:30:00Z",
    "updatedAt": "2024-01-15T10:30:00Z"
  }
]
```

---

## 2. GET RESIDENCE BY ID

**Request:**
```bash
curl -X GET https://localhost:7000/api/residences/1 -k
```

**Expected Response (200 OK):**
```json
{
  "id": 1,
  "name": "Downtown Apartment",
  "address": "123 Main St",
  "city": "New York",
  "state": "NY",
  "zipCode": "10001",
  "numberOfRooms": 2,
  "price": 250000,
  "description": "Modern apartment",
  "createdAt": "2024-01-15T10:30:00Z",
  "updatedAt": "2024-01-15T10:30:00Z"
}
```

**Not Found (404):**
```bash
curl -X GET https://localhost:7000/api/residences/999 -k
```

Response:
```
404 Not Found
```

**Invalid ID (400):**
```bash
curl -X GET https://localhost:7000/api/residences/0 -k
```

Response:
```json
"ID must be greater than 0"
```

---

## 3. CREATE NEW RESIDENCE

**Request:**
```bash
curl -X POST https://localhost:7000/api/residences \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Beach House",
    "address": "789 Beach Road",
    "city": "Miami",
    "state": "FL",
    "zipCode": "33101",
    "numberOfRooms": 5,
    "price": 750000.00,
    "description": "Luxury beachfront property"
  }' -k
```

**Expected Response (201 Created):**
```
Location: /api/residences/4

{
  "id": 4,
  "name": "Beach House",
  "address": "789 Beach Road",
  "city": "Miami",
  "state": "FL",
  "zipCode": "33101",
  "numberOfRooms": 5,
  "price": 750000.00,
  "description": "Luxury beachfront property",
  "createdAt": "2024-01-15T11:45:00Z",
  "updatedAt": "2024-01-15T11:45:00Z"
}
```

**Invalid Input - Missing Name (400):**
```bash
curl -X POST https://localhost:7000/api/residences \
  -H "Content-Type: application/json" \
  -d '{
    "address": "789 Beach Road",
    "city": "Miami",
    "state": "FL",
    "zipCode": "33101",
    "numberOfRooms": 5,
    "price": 750000.00
  }' -k
```

Response:
```json
"Name is required"
```

**Invalid Input - Invalid Price (400):**
```bash
curl -X POST https://localhost:7000/api/residences \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Beach House",
    "address": "789 Beach Road",
    "city": "Miami",
    "state": "FL",
    "zipCode": "33101",
    "numberOfRooms": 5,
    "price": -1000.00
  }' -k
```

Response:
```json
"Price must be greater than 0"
```

---

## 4. UPDATE RESIDENCE

**Request:**
```bash
curl -X PUT https://localhost:7000/api/residences/1 \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Updated Downtown Apartment",
    "address": "123 Main St Suite 200",
    "city": "New York",
    "state": "NY",
    "zipCode": "10001",
    "numberOfRooms": 3,
    "price": 275000.00,
    "description": "Renovated apartment with modern amenities"
  }' -k
```

**Expected Response (200 OK):**
```json
{
  "id": 1,
  "name": "Updated Downtown Apartment",
  "address": "123 Main St Suite 200",
  "city": "New York",
  "state": "NY",
  "zipCode": "10001",
  "numberOfRooms": 3,
  "price": 275000.00,
  "description": "Renovated apartment with modern amenities",
  "createdAt": "2024-01-15T10:30:00Z",
  "updatedAt": "2024-01-15T12:00:00Z"
}
```

**Not Found (404):**
```bash
curl -X PUT https://localhost:7000/api/residences/999 \
  -H "Content-Type: application/json" \
  -d '{...}' -k
```

Response:
```
404 Not Found
```

**Invalid ID (400):**
```bash
curl -X PUT https://localhost:7000/api/residences/0 \
  -H "Content-Type: application/json" \
  -d '{...}' -k
```

Response:
```json
"ID must be greater than 0"
```

---

## 5. DELETE RESIDENCE

**Request:**
```bash
curl -X DELETE https://localhost:7000/api/residences/1 -k
```

**Expected Response (204 No Content):**
```
(Empty body)
```

**Not Found (404):**
```bash
curl -X DELETE https://localhost:7000/api/residences/999 -k
```

Response:
```
404 Not Found
```

**Invalid ID (400):**
```bash
curl -X DELETE https://localhost:7000/api/residences/0 -k
```

Response:
```json
"ID must be greater than 0"
```

---

## Testing with PowerShell

### GET ALL
```powershell
$response = Invoke-RestMethod -Uri "https://localhost:7000/api/residences" -Method Get -SkipCertificateCheck
$response | ConvertTo-Json | Write-Host
```

### CREATE
```powershell
$body = @{
    name = "Beach House"
    address = "789 Beach Road"
    city = "Miami"
    state = "FL"
    zipCode = "33101"
    numberOfRooms = 5
    price = 750000.00
    description = "Luxury beachfront property"
} | ConvertTo-Json

$response = Invoke-RestMethod -Uri "https://localhost:7000/api/residences" `
    -Method Post `
    -Body $body `
    -ContentType "application/json" `
    -SkipCertificateCheck

$response | ConvertTo-Json | Write-Host
```

### UPDATE
```powershell
$body = @{
    name = "Updated Downtown Apartment"
    address = "123 Main St Suite 200"
    city = "New York"
    state = "NY"
    zipCode = "10001"
    numberOfRooms = 3
    price = 275000.00
    description = "Renovated apartment"
} | ConvertTo-Json

$response = Invoke-RestMethod -Uri "https://localhost:7000/api/residences/1" `
    -Method Put `
    -Body $body `
    -ContentType "application/json" `
    -SkipCertificateCheck

$response | ConvertTo-Json | Write-Host
```

### DELETE
```powershell
$response = Invoke-RestMethod -Uri "https://localhost:7000/api/residences/1" `
    -Method Delete `
    -SkipCertificateCheck

Write-Host "Deleted successfully"
```

---

## Testing with Postman

### 1. Import Collection

Create a new Postman collection:

```json
{
  "info": {
    "name": "Residence API",
    "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
  },
  "item": [
    {
      "name": "Get All Residences",
      "request": {
        "method": "GET",
        "url": "https://localhost:7000/api/residences"
      }
    },
    {
      "name": "Get Residence by ID",
      "request": {
        "method": "GET",
        "url": "https://localhost:7000/api/residences/1"
      }
    },
    {
      "name": "Create Residence",
      "request": {
        "method": "POST",
        "header": [
          {
            "key": "Content-Type",
            "value": "application/json"
          }
        ],
        "body": {
          "mode": "raw",
          "raw": "{...}"
        },
        "url": "https://localhost:7000/api/residences"
      }
    },
    {
      "name": "Update Residence",
      "request": {
        "method": "PUT",
        "header": [
          {
            "key": "Content-Type",
            "value": "application/json"
          }
        ],
        "body": {
          "mode": "raw",
          "raw": "{...}"
        },
        "url": "https://localhost:7000/api/residences/1"
      }
    },
    {
      "name": "Delete Residence",
      "request": {
        "method": "DELETE",
        "url": "https://localhost:7000/api/residences/1"
      }
    }
  ]
}
```

### 2. Settings

- **SSL Certificate Verification**: Disable (for local development)
- **Base URL**: `https://localhost:7000`

---

## Validation Test Cases

### Test 1: Create with Missing Required Fields

```bash
# Missing Name
curl -X POST https://localhost:7000/api/residences \
  -H "Content-Type: application/json" \
  -d '{"address": "123 St", "city": "NYC", "state": "NY", "zipCode": "10001", "numberOfRooms": 2, "price": 100000}' -k

# Expected: 400 - "Name is required"
```

### Test 2: Create with Invalid Numbers

```bash
# Invalid Price (0)
curl -X POST https://localhost:7000/api/residences \
  -H "Content-Type: application/json" \
  -d '{"name": "Home", "address": "123 St", "city": "NYC", "state": "NY", "zipCode": "10001", "numberOfRooms": 2, "price": 0}' -k

# Expected: 400 - "Price must be greater than 0"
```

### Test 3: Update Non-Existent Resource

```bash
curl -X PUT https://localhost:7000/api/residences/999 \
  -H "Content-Type: application/json" \
  -d '{"name": "Home", ...}' -k

# Expected: 404 Not Found
```

### Test 4: Delete Non-Existent Resource

```bash
curl -X DELETE https://localhost:7000/api/residences/999 -k

# Expected: 404 Not Found
```

---

## Swagger UI Testing

1. Navigate to: `https://localhost:7000/swagger`
2. All endpoints are displayed with:
   - Request parameters
   - Request body schema
   - Response schemas
   - Try It Out button
3. Click "Try It Out" on any endpoint
4. Fill in parameters/body
5. Click "Execute"
6. View response

---

## Response Status Codes

| Endpoint | Method | Success | Client Error | Server Error |
|----------|--------|---------|--------------|--------------|
| /api/residences | GET | 200 OK | - | 500 |
| /api/residences/{id} | GET | 200 OK | 400, 404 | 500 |
| /api/residences | POST | 201 Created | 400 | 500 |
| /api/residences/{id} | PUT | 200 OK | 400, 404 | 500 |
| /api/residences/{id} | DELETE | 204 No Content | 400, 404 | 500 |

---

## Common Response Headers

```
Content-Type: application/json
Content-Length: 1234
Date: Mon, 15 Jan 2024 10:30:00 GMT
Server: Kestrel
Transfer-Encoding: chunked
Connection: keep-alive
```

**For POST (201 Created):**
```
Location: /api/residences/4
```

---

## Performance Testing

### Load Test with Apache Bench
```bash
ab -n 1000 -c 10 https://localhost:7000/api/residences/
```

### Load Test with wrk
```bash
wrk -t4 -c100 -d30s https://localhost:7000/api/residences
```

---

## Notes

- All timestamps are in UTC (ISO 8601 format)
- Prices are stored with 2 decimal places
- Database automatically sets `CreatedAt` and `UpdatedAt`
- All string properties use Unicode (nvarchar in SQL Server)
- Resource IDs are auto-incrementing integers
