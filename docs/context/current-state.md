# Current State

- Bieżąca gałąź robocza: `feature/stage1-dto-mapping`.
- Backend składa się z czterech projektów: `AppCore`, `Infrastructure`, `WebApp`, `UnitTest`.
- W `AppCore/Dto` dodano jawne metody mapujące (`FromEntity`, `ToEntity`, `FromEntities`) dla wszystkich DTO używanych w aktualnym modelu domenowym parkingu.
- `Infrastructure` nadal korzysta z repozytoriów in-memory.
- `WebApp` zawiera jeszcze tylko przykładowy endpoint `weatherforecast`, bez właściwych endpointów domenowych parkingu.
- Testy obejmują repozytoria pamięciowe oraz mapowanie DTO.