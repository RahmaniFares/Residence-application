# UML Diagrams - Visual Guide (Mermaid Format)

## 1. Domain Entities Class Diagram

```mermaid
classDiagram
    direction TB

    class BaseEntity {
        -Guid Id
        -DateTime CreatedAt
        -DateTime? UpdatedAt
        -bool IsDeleted
    }

    class Residence {
        -Guid Id
        -string Name
        -string Address
        -string City
        -string State
        -string ZipCode
        -string Description
        --
        +1 -> * Tarif
        +1 -> * House
        +1 -> * Resident
        +1 -> * TarifHistory
    }

    class Tarif {
        -Guid ResidenceId
        -string Description
        -decimal Amount
        -string Currency
        -DateTime EffectiveDate
        -DateTime? EndDate
        -bool IsActive
        -string? Notes
        --
        +Guid ResidenceId (FK)
        +1 -> * TarifHistory
        +Residence -* contains
    }

    class TarifHistory {
        -Guid TarifId
        -Guid ResidenceId
        -decimal PreviousAmount
        -decimal NewAmount
        -string PreviousDescription
        -string NewDescription
        -DateTime EffectiveDate
        -string ChangedBy
        -string? ChangeReason
        --
        +* <- 1 Tarif
        +* <- 1 Residence
    }

    class House {
        -string Block
        -string Unit
        -string? Floor
        -HouseStatus Status
        -Guid? CurrentResidentId
        --
        +1 -> * Rappel
        +1 -> * Payment
        +1 -> * Resident
        +* <- 1 Residence
    }

    class Rappel {
        -Guid HouseId
        -decimal Amount
        -RappelStatus Status
        -DateTime? PaymentDate
        -string? Notes
        --
        +* <- 1 House
    }

    class Payment {
        -Guid HouseId
        -Guid ResidentId
        -decimal Amount
        -PaymentMethod Method
        -DateTime PeriodStart
        -DateTime PeriodEnd
        -DateTime? PaymentDate
        -PaymentStatus Status
        -string? Notes
        --
        +* <- 1 House
        +* <- 1 Resident
    }

    class Resident {
        -string FirstName
        -string LastName
        -string Email
        -string? Phone
        --
        +* -> 1 House
        +* <- 1 Payment
    }

    class HouseStatus {
        <<enumeration>>
        Vacant
        Occupied
        UnderMaintenance
        Reserved
    }

    class RappelStatus {
        <<enumeration>>
        Unpaid
        Paid
    }

    class PaymentStatus {
        <<enumeration>>
        Pending
        Paid
        Overdue
    }

    class PaymentMethod {
        <<enumeration>>
        Transfer
        Check
        Cash
        Card
    }

    BaseEntity <|-- Tarif
    BaseEntity <|-- TarifHistory
    BaseEntity <|-- House
    BaseEntity <|-- Rappel
    BaseEntity <|-- Payment
    BaseEntity <|-- Resident

    Residence "1" --> "*" Tarif
    Residence "1" --> "*" House
    Residence "1" --> "*" Resident
    Residence "1" --> "*" TarifHistory

    Tarif "1" --> "*" TarifHistory

    House "1" --> "*" Rappel
    House "1" --> "*" Payment
    House "1" --> "*" Resident

    Payment "1" --> "*" Resident

    House --> HouseStatus
    Rappel --> RappelStatus
    Payment --> PaymentStatus
    Payment --> PaymentMethod
```

---

## 2. Service & Repository Architecture

```mermaid
classDiagram
    direction TB

    class ITarifService {
        <<interface>>
        +CreateTarifAsync(Guid, CreateTarifDto, string): Task~TarifDto~
        +UpdateTarifAsync(Guid, Guid, UpdateTarifDto, string): Task~TarifDto~
        +GetTarifByIdAsync(Guid): Task~TarifDto?~
        +GetTarifsByResidenceAsync(Guid): Task~IEnumerable~TarifDto~~
        +GetCurrentTarifAsync(Guid): Task~TarifDto?~
        +GetTarifHistoryAsync(Guid): Task~IEnumerable~TarifHistoryDto~~
        +UpdateTarifHistoryAsync(...): Task~TarifHistoryDto~
        +DeleteTarifAsync(Guid, Guid): Task~bool~
    }

    class IRappelService {
        <<interface>>
        +CreateRappelAsync(Guid, CreateRappelDto): Task~RappelDto~
        +GetRappelByIdAsync(Guid): Task~RappelDto~
        +UpdateRappelAsync(Guid, UpdateRappelDto): Task~RappelDto~
        +DeleteRappelAsync(Guid): Task
        +GetRappelsByHouseAsync(Guid, PaginationDto): Task~PagedResultDto~RappelDto~~
        +GetRappelsByResidenceAsync(Guid, PaginationDto): Task~PagedResultDto~RappelDto~~
    }

    class TarifService {
        -_tarifRepository: ITarifRepository
        -_tarifHistoryRepository: ITarifHistoryRepository
        -_residenceRepository: IResidenceRepository
        -_houseRepository: IHouseRepository
        -_paymentRepository: IPaymentRepository
        -_rappelRepository: IRappelRepository
        --
        +CreateTarifAsync(): Task~TarifDto~
        +UpdateTarifAsync(): Task~TarifDto~
        #DetectAndCreateRappelsAsync(): Task
        #MapToDto(): TarifDto
        #MapHistoryToDto(): TarifHistoryDto
    }

    class RappelService {
        -_rappelRepository: IRappelRepository
        -_houseRepository: IHouseRepository
        --
        +CreateRappelAsync(): Task~RappelDto~
        +GetRappelByIdAsync(): Task~RappelDto~
        +UpdateRappelAsync(): Task~RappelDto~
        +DeleteRappelAsync(): Task
        #MapToDto(): RappelDto
    }

    class ITarifRepository {
        <<interface>>
        +GetByIdAsync(Guid): Task~Tarif?~
        +GetCurrentTarifAsync(Guid): Task~Tarif?~
        +GetTarifsByResidenceAsync(Guid): Task~IEnumerable~Tarif~~
        +AddAsync(Tarif): Task~Tarif~
        +UpdateAsync(Tarif): Task
        +DeleteAsync(Guid): Task
    }

    class ITarifHistoryRepository {
        <<interface>>
        +GetByIdAsync(Guid): Task~TarifHistory?~
        +GetHistoryByTarifIdAsync(Guid): Task~IEnumerable~TarifHistory~~
        +GetHistoryByResidenceIdAsync(Guid): Task~IEnumerable~TarifHistory~~
        +AddAsync(TarifHistory): Task~TarifHistory~
        +UpdateAsync(TarifHistory): Task
    }

    class IHouseRepository {
        <<interface>>
        +GetByResidenceWithDetailsAsync(Guid): Task~IEnumerable~House~~
        +GetByIdAsync(Guid): Task~House?~
    }

    class IPaymentRepository {
        <<interface>>
        +GetByHouseAsync(Guid): Task~IEnumerable~Payment~~
    }

    class IRappelRepository {
        <<interface>>
        +GetByHouseAsync(Guid): Task~IEnumerable~Rappel~~
        +AddAsync(Rappel): Task~Rappel~
        +SaveChangesAsync(): Task
    }

    ITarifService <|.. TarifService
    IRappelService <|.. RappelService

    TarifService --> ITarifRepository
    TarifService --> ITarifHistoryRepository
    TarifService --> IHouseRepository
    TarifService --> IPaymentRepository
    TarifService --> IRappelRepository

    RappelService --> IRappelRepository
    RappelService --> IHouseRepository
```

---

## 3. Rappel Detection Sequence Diagram

```mermaid
sequenceDiagram
    actor Admin as Admin/API
    participant Controller as TarifController
    participant Service as TarifService
    participant TarifRepo as TarifRepository
    participant HouseRepo as HouseRepository
    participant PayRepo as PaymentRepository
    participant RappelRepo as RappelRepository
    participant DB as Database

    Admin->>Controller: PUT /tarifs/{id}<br/>UpdateTarifDto
    Controller->>Service: UpdateTarifAsync()
    Service->>TarifRepo: GetByIdAsync(tarifId)
    TarifRepo->>DB: SELECT * FROM Tarif
    DB-->>TarifRepo: Tarif
    TarifRepo-->>Service: Current Tarif

    Service->>Service: Store oldAmount<br/>Check amountChanged

    alt Amount Increased (> oldAmount)
        Service->>TarifRepo: AddAsync(history)
        TarifRepo->>DB: INSERT INTO TarifHistory

        Service->>TarifRepo: UpdateAsync(tarif)
        TarifRepo->>DB: UPDATE Tarif

        Service->>HouseRepo: GetByResidenceWithDetailsAsync()
        HouseRepo->>DB: SELECT * FROM House
        DB-->>HouseRepo: Houses[]
        HouseRepo-->>Service: Houses[]

        loop For Each House
            Service->>PayRepo: GetByHouseAsync(houseId)
            PayRepo->>DB: SELECT * FROM Payment
            DB-->>PayRepo: Payments[]
            PayRepo-->>Service: Payments[]

            Service->>Service: Filter Pre-Paid Months<br/>(PeriodEnd >= effectiveDate,<br/>Status = Paid)

            Service->>Service: Calculate:<br/>- Affected Months<br/>- Delta<br/>- Rappel Amount

            Service->>RappelRepo: GetByHouseAsync(houseId)
            RappelRepo->>DB: SELECT * FROM Rappel
            DB-->>RappelRepo: Rappels[]
            RappelRepo-->>Service: Rappels[]

            alt No Unpaid Rappel Exists
                Service->>RappelRepo: AddAsync(newRappel)
                RappelRepo->>DB: INSERT INTO Rappel
                Note over RappelRepo: ✅ Rappel Created
            else Unpaid Rappel Exists
                Note over Service: ⏭️ Skip - Duplicate Prevention
            end
        end

        Service->>RappelRepo: SaveChangesAsync()
        RappelRepo->>DB: COMMIT Transaction

    else Amount Decreased or No Change
        Note over Service: History Recorded<br/>No Rappels Created
    end

    Service->>Service: MapToDto(tarif)
    Service-->>Controller: TarifDto
    Controller-->>Admin: 200 OK - TarifDto
```

---

## 4. Component Architecture

```mermaid
graph TB
    subgraph API["API Layer"]
        TarifAPI[TarifEndpoints]
        RappelAPI[RappelEndpoints]
    end

    subgraph Application["Application Layer"]
        TarifService[TarifService]
        RappelService[RappelService]
        DTOLayer[DTOs & Mappers]
    end

    subgraph Domain["Domain Layer"]
        Entities[Entities<br/>Tarif, House, Payment,<br/>Rappel, TarifHistory]
        Enums[Enums<br/>HouseStatus, RappelStatus,<br/>PaymentStatus, PaymentMethod]
    end

    subgraph DataAccess["Data Access Layer"]
        TarifRepo[TarifRepository]
        HistoryRepo[TarifHistoryRepository]
        HouseRepo[HouseRepository]
        PaymentRepo[PaymentRepository]
        RappelRepo[RappelRepository]
    end

    subgraph Database["Database"]
        DB[(SQL Server<br/>Database)]
        Tables["Tables: Tarif, TarifHistory,<br/>House, Payment, Rappel"]
    end

    TarifAPI --> TarifService
    RappelAPI --> RappelService

    TarifService --> DTOLayer
    RappelService --> DTOLayer

    TarifService --> Entities
    RappelService --> Entities
    Entities --> Enums

    TarifService --> TarifRepo
    TarifService --> HistoryRepo
    TarifService --> HouseRepo
    TarifService --> PaymentRepo
    TarifService --> RappelRepo

    TarifRepo --> DB
    HistoryRepo --> DB
    HouseRepo --> DB
    PaymentRepo --> DB
    RappelRepo --> DB

    DB --> Tables
```

---

## 5. Rappel Detection Algorithm Flow

```mermaid
flowchart TD
    Start([UpdateTarifAsync Called]) --> ValidateTarif{Tarif<br/>Exists?}

    ValidateTarif -->|No| ReturnError1["❌ Error: Tariff not found"]
    ValidateTarif -->|Yes| CheckResidence{Belongs to<br/>Residence?}

    CheckResidence -->|No| ReturnError2["❌ Error: Wrong residence"]
    CheckResidence -->|Yes| StoreOldAmount["📝 Store old amount"]

    StoreOldAmount --> CheckAmountChanged{Amount<br/>Changed?}
    CheckAmountChanged -->|Yes| RecordHistory["📋 Record history"]
    CheckAmountChanged -->|No| UpdateTarif1["🔄 Update tariff fields"]

    RecordHistory --> UpdateTarif1
    UpdateTarif1 --> CommitDB1["💾 Commit tariff to DB"]

    CommitDB1 --> CheckAmountIncreased{Amount<br/>Increased?}

    CheckAmountIncreased -->|No| ReturnSuccess1["✅ Return TarifDto"]
    CheckAmountIncreased -->|Yes| GetHouses["🏘️ Get all houses"]

    GetHouses --> LoopHouses{"For each<br/>House"}
    LoopHouses --> GetPayments["💳 Get house payments"]

    GetPayments --> FilterPrePaid["🔍 Filter pre-paid months<br/>PeriodEnd >= effectiveDate<br/>Status = Paid"]

    FilterPrePaid --> CheckPrePaid{Pre-paid<br/>Exist?}
    CheckPrePaid -->|No| NextHouse["⏭️ Next house"]
    CheckPrePaid -->|Yes| CalcMonths["📊 Calculate affected months"]

    CalcMonths --> CalcDelta["⚖️ Calculate delta<br/>newAmount - oldAmount"]

    CalcDelta --> CheckDelta{Delta > 0?}
    CheckDelta -->|No| NextHouse
    CheckDelta -->|Yes| CheckExisting["🔎 Check existing<br/>unpaid rappel"]

    CheckExisting --> HasUnpaid{Unpaid<br/>Rappel?}
    HasUnpaid -->|Yes| NextHouse
    HasUnpaid -->|No| CalcAmount["🧮 Rappel Amount =<br/>Delta × Months"]

    CalcAmount --> CreateRappel["➕ Create rappel record<br/>Status = Unpaid"]
    CreateRappel --> AddToDB["➕ Add to DB"]
    AddToDB --> NextHouse

    NextHouse --> MoreHouses{More<br/>Houses?}
    MoreHouses -->|Yes| GetPayments
    MoreHouses -->|No| CommitRappels["💾 Commit all rappels"]

    CommitRappels --> ReturnSuccess2["✅ Return TarifDto"]
    ReturnSuccess1 --> End([End])
    ReturnSuccess2 --> End
    ReturnError1 --> End
    ReturnError2 --> End
```

---

## 6. Data Entity Relationships

```mermaid
erDiagram
    RESIDENCE ||--o{ TARIF : contains
    RESIDENCE ||--o{ TARIFHISTORY : tracks
    RESIDENCE ||--o{ HOUSE : contains
    RESIDENCE ||--o{ RESIDENT : has

    TARIF ||--o{ TARIFHISTORY : has

    HOUSE ||--o{ RAPPEL : generates
    HOUSE ||--o{ PAYMENT : receives
    HOUSE ||--o{ RESIDENT : occupies

    PAYMENT }|--|| RESIDENT : from
    PAYMENT }|--|| HOUSE : for

    RAPPEL }|--|| HOUSE : for

    TARIF {
        guid id
        guid residenceId FK
        string description
        decimal amount
        string currency
        datetime effectiveDate
        datetime endDate
        boolean isActive
    }

    TARIFHISTORY {
        guid id
        guid tarifId FK
        guid residenceId FK
        decimal previousAmount
        decimal newAmount
        string previousDescription
        string newDescription
        datetime effectiveDate
        string changedBy
        string changeReason
    }

    RESIDENCE {
        guid id
        string name
        string address
        string city
    }

    HOUSE {
        guid id
        guid residenceId FK
        string block
        string unit
        string floor
        int status
        guid currentResidentId FK
    }

    RAPPEL {
        guid id
        guid houseId FK
        decimal amount
        int status
        datetime paymentDate
        string notes
    }

    PAYMENT {
        guid id
        guid houseId FK
        guid residentId FK
        decimal amount
        int method
        datetime periodStart
        datetime periodEnd
        datetime paymentDate
        int status
    }

    RESIDENT {
        guid id
        string firstName
        string lastName
        string email
    }
```

---

## 7. Dependency Injection Graph

```mermaid
graph LR
    API["TarifEndpoints"]

    API -->|Injects| Service["TarifService"]
    Service -->|Uses| TarifRepo["ITarifRepository"]
    Service -->|Uses| HistoryRepo["ITarifHistoryRepository"]
    Service -->|Uses| ResidenceRepo["IResidenceRepository"]
    Service -->|Uses| HouseRepo["IHouseRepository"]
    Service -->|Uses| PaymentRepo["IPaymentRepository"]
    Service -->|Uses| RappelRepo["IRappelRepository"]

    TarifRepo -->|Implements| TarifRepoImpl["TarifRepository"]
    HistoryRepo -->|Implements| HistoryRepoImpl["TarifHistoryRepository"]
    HouseRepo -->|Implements| HouseRepoImpl["HouseRepository"]
    PaymentRepo -->|Implements| PaymentRepoImpl["PaymentRepository"]
    RappelRepo -->|Implements| RappelRepoImpl["RappelRepository"]

    TarifRepoImpl -->|Uses| EFCore["Entity Framework Core"]
    HistoryRepoImpl -->|Uses| EFCore
    HouseRepoImpl -->|Uses| EFCore
    PaymentRepoImpl -->|Uses| EFCore
    RappelRepoImpl -->|Uses| EFCore

    EFCore -->|Connects| Database["SQL Server Database"]

    style Service fill:#E6FFE6
    style TarifRepo fill:#E6F3FF
    style HistoryRepo fill:#E6F3FF
    style HouseRepo fill:#E6F3FF
    style PaymentRepo fill:#E6F3FF
    style RappelRepo fill:#E6F3FF
    style Database fill:#FFE6CC
```

---

## 8. Update Tariff Processing States

```mermaid
stateDiagram-v2
    [*] --> RequestReceived

    RequestReceived --> ValidatingTarif: Check tariff exists
    ValidatingTarif --> ResidenceCheck: Verify residence

    ResidenceCheck -->|Failed| ErrorState: ❌ Invalid residence
    ResidenceCheck -->|Success| StoringAmount: Store old amount

    StoringAmount --> CheckingChange: Detect amount change
    CheckingChange -->|No change| UpdatingOtherFields: Update other fields only
    CheckingChange -->|Amount changed| RecordingHistory: 📋 Record history

    RecordingHistory --> UpdatingOtherFields
    UpdatingOtherFields --> CommittingTarif: 💾 Commit to DB

    CommittingTarif --> CheckingIncrease: Amount increased?
    CheckingIncrease -->|No/Decreased| ReturningResult: ✅ Return result
    CheckingIncrease -->|Yes/Increased| DetectingRappels: 🔍 Detect rappels

    DetectingRappels --> IteratingHouses: For each house...
    IteratingHouses --> FilteringPrepaid: Filter pre-paid months
    FilteringPrepaid --> CalculatingAffected: Calculate affected months
    CalculatingAffected --> CalculatingDelta: Calculate delta
    CalculatingDelta --> CheckingDuplicate: Check for duplicate rappel

    CheckingDuplicate -->|Duplicate exists| NextHouse: ⏭️ Skip
    CheckingDuplicate -->|No duplicate| CreatingRappel: ➕ Create rappel

    CreatingRappel --> AddingToDb: ➕ Add to DB
    AddingToDb --> NextHouse

    NextHouse -->|More houses| IteratingHouses
    NextHouse -->|All done| CommittingRappels: 💾 Commit all
    CommittingRappels --> ReturningResult

    ReturningResult --> [*]
    ErrorState --> [*]
```

---

## 9. Rappel Creation Decision Tree

```mermaid
graph TD
    A["UpdateTarif Called<br/>oldAmount=100, newAmount=120"] --> B{Amount<br/>Changed?}
    B -->|No| C["✓ Update other fields<br/>✗ No rappel detection"]
    B -->|Yes| D{New > Old<br/>Amount?}
    D -->|Decrease/Equal| E["✓ Record history<br/>✗ No rappel detection"]
    D -->|Increase| F["Houses in Residence?"]
    F -->|No| G["✗ No rappels created"]
    F -->|Yes| H["For Each House"]
    H --> I["Payments with<br/>PeriodEnd >= EffectiveDate<br/>Status = Paid?"]
    I -->|No| J["✗ No rappel for house"]
    I -->|Yes| K["Calculate<br/>Affected Months Count"]
    K --> L["Delta = New - Old<br/>=120-100=20"]
    L --> M["Rappel Amt =<br/>Delta × Months<br/>=20×3=60"]
    M --> N{Unpaid<br/>Rappel<br/>Exists?}
    N -->|Yes| O["✗ Skip<br/>Duplicate Prevention"]
    N -->|No| P["✓ Create Rappel<br/>Amount: 60<br/>Status: Unpaid"]
    P --> Q["Save to Database"]

    style A fill:#FFE6E6
    style C fill:#E6FFE6
    style E fill:#E6FFE6
    style G fill:#FFE6CC
    style J fill:#FFE6CC
    style O fill:#FFE6CC
    style P fill:#E6FFE6
    style Q fill:#E6F3FF
```

---

## 10. Database Schema Relationships

```mermaid
graph LR
    Tarif["⬜ Tarif<br/>id, residenceId*,<br/>description, amount,<br/>currency, effectiveDate"]

    TarifHistory["⬜ TarifHistory<br/>id, tarifId*, residenceId*,<br/>previousAmount, newAmount,<br/>effectiveDate, changedBy"]

    Residence["⬜ Residence<br/>id, name, address"]

    House["⬜ House<br/>id, residenceId*, block,<br/>unit, floor, status"]

    Rappel["⬜ Rappel<br/>id, houseId*, amount,<br/>status, paymentDate"]

    Payment["⬜ Payment<br/>id, houseId*, residentId*,<br/>amount, periodStart,<br/>periodEnd, status"]

    Resident["⬜ Resident<br/>id, firstName, lastName"]

    Tarif -->|many to one| Residence
    TarifHistory -->|many to one| Tarif
    TarifHistory -->|references| Residence
    House -->|many to one| Residence
    Rappel -->|many to one| House
    Payment -->|many to one| House
    Payment -->|many to one| Resident

    style Tarif fill:#FFE6E6
    style TarifHistory fill:#FFE6E6
    style Rappel fill:#FFFACD
    style Payment fill:#E6F3FF
    style House fill:#E6FFE6
    style Residence fill:#F0E6FF
```

---

## Summary

These Mermaid diagrams provide:

✅ **Complete class hierarchies** with all properties and methods  
✅ **Service architecture** showing interfaces and implementations  
✅ **Sequence diagrams** detailing the rappel detection flow  
✅ **Entity relationships** in ER diagram format  
✅ **Processing flow charts** for complex business logic  
✅ **State machines** showing system state transitions  
✅ **Decision trees** for rappel creation logic  
✅ **Component architecture** with layered design  

You can copy these Mermaid diagrams directly into:
- GitHub README files (rendered automatically)
- Markdown tools that support Mermaid
- Mermaid Live Editor: https://mermaid.live/
- VS Code with Markdown Preview Enhanced extension
