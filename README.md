## Predpogoji

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Docker (Docker Desktop (Windows, macOS) ali Docker Engine (linux)) za PostgreSQL
- `dotnet-ef` orodje za migracije:
  ```sh
  dotnet tool install --global dotnet-ef
  ```

## 1. Zagon SQL strežnika

```sh
docker compose up -d
```

Zažene PostgreSQL 17 na `localhost:5432` z bazo `humanfrogapi`, uporabnikom `postgres`, geslom `postgres`.

Connection string je v `src/HumanFrogApi/appsettings.json` (`ConnectionStrings:DefaultConnection`).

## 2. Izdelava sheme

Enkratni zagon za novo bazo:

```sh
dotnet ef database update --project src/HumanFrogApi
```

## 3. Zagon API

```sh
dotnet run --project src/HumanFrogApi --launch-profile http
```

Api posluša na `http://localhost:5131`. `https` profil dodatno posluša na `https://localhost:7017`.

V `Development` okolju (privzeto za oba zagonska profila):

- Napolni bazo s podatki iz `src/HumanFrogApi/Data/DbSeeder.cs` razen če že obstajajo
- Servira dokumentacijo OpenAPI na `http://localhost:5131/openapi/v1.json`.

## Končne točke

| Method | Path                          | Description                                                                 |
| ------ | ----------------------------- | --------------------------------------------------------------------------- |
| GET    | `/events?from=&to=`           | Seznam dogodkov po datumih. `from` in `to` sta opcijska ISO datuma.        |
| GET    | `/events/{id}`                | Vse podatke o dogodku, vključno s prostimi mesti (`prostaMesta`).          |
| POST   | `/events/{id}/registrations`  | Prijava na dogodek. Telo: `{ "name": "", "surname": "", "email": "" }`. |

## Uporaba AI
AI je pri tej nalogi bil uporabljen za začetek projekta (scafolding). Same entitete, controllerji in service-i so napisani ročno. Prav tako je bil uporabljen za končni pregled kode in predloge za izboljšanje. Na koncu pa je bil uporabljen še za izdelavo `src/HumanFrogApi/DbSeeder.cs` za izmišljene testne podatke.

## Odločitve
Za bazo sem se odločil za PostgreSQL zaradi čisto osebne preference, ker za majhne projekte vedno uporabim PostgreSQL.

Modele sem razdelil na Requests in Responses za bolj strukturirane in ločene definicije razredov zahtevkov in odgovorov.

Same entitete in njihove vrednosti so napisane v slovenščini saj sem točno sledil dokumentaciji naloge, kjer so bila polja napisana v slovenščini.

## Izboljšave
Za /events endpoint bi že v začetku dodal ali je dogodek poln ali ne, saj bi s tem lahko na front-endu lahko vnaprej prikazal ali se je možno prijaviti na dogodek ali ne. 

Dodal bi tudi avtomatično arhiviranje preteklih dogodkov (npr. po dveh tednih od začetka dogodka), saj nima smisla prikazovati zelo starih dogodkov.

Zelo pomembno bi bilo tudi dodati odjavo od dogodkov.

Če pa bi aplikacija šla naprej v produkcijsko okolje pa bi bilo minimalno potrebno narediti uporabniške in administratorske profile z opcijo dodajanja dogodkov.