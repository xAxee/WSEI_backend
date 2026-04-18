# Current State

- Bieżąca gałąź robocza: `feature/stage1-memory-repositories`.
- Backend składa się z czterech projektów: `AppCore`, `Infrastructure`, `WebApp`, `UnitTest`.
- W `AppCore/Dto` dodano jawne metody mapujące (`FromEntity`, `ToEntity`, `FromEntities`) dla wszystkich DTO używanych w aktualnym modelu domenowym parkingu.
- `Infrastructure` korzysta z repozytoriów in-memory opartych o `MemoryGenericRepository`.
- Dostępne są już konkretne implementacje pamięciowe dla `Vehicle`, `ParkingGate` i `ParkingSession`, każda z przykładowymi danymi startowymi w konstruktorze.
- `WebApp` zawiera jeszcze tylko przykładowy endpoint `weatherforecast`, bez właściwych endpointów domenowych parkingu.
- Testy obejmują repozytoria pamięciowe oraz mapowanie DTO.