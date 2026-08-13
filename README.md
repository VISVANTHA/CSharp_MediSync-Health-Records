# CSharp_MediSync-Health-Records

**Project:** MediSync Health Records  
**Language:** C#  
**Branch:** `CSharp_FE_V4.7.2_BE_V10.0`  
**Frontend version:** 4.7.2  
**Backend version:** 10.0

## Reference URLs

- `frontend_url`: https://learn.microsoft.com/en-us/aspnet/core/grpc/grpcweb
- `backend_url`: https://learn.microsoft.com/en-us/aspnet/core/grpc/grpcweb
- Frontend component: MediSync Patient Portal (Blazor WebAssembly)
- Backend component: MediSync Records Service (gRPC-Web)

## Layout

- `frontend_csharp/` — frontend service
- `backend_csharp/` — backend API

## How to run (backend → frontend pipeline)

1. Start the backend in `backend_csharp/` (see that module's README / start script).
2. Start the frontend in `frontend_csharp/`.
3. Confirm the frontend successfully receives data from the backend endpoint.

## Tools

Each module has `run_tools.ps1` / `run_tools.sh` that invokes the Scenario 2 tool set and writes artifacts under `tool-output/`. Failures abort with the tool name — they do not silently skip.

Tools for this language (12):
- unilyze (verified: 8)
- Dolos (verified: N/A (language/runtime-version agnostic))
- Opengrep (verified: 12)
- Opengrep (unilyze wrong — code-health/complexity, not OWASP/secure-coding SAST) (verified: 12)
- Opengrep (unilyze wrong — no input-validation or taint analysis) (verified: 12)
- Opengrep (unilyze wrong — no secrets/dataflow/taint tracking) (verified: 12)
- Opengrep (unilyze wrong — no auth/access-control analysis) (verified: 12)
- Trivy (unilyze wrong — no NuGet/CVE/supply-chain scanning) (verified: N/A (language/runtime-version agnostic))
- Trivy (verified: N/A (language/runtime-version agnostic))
- MiniCover (verified: 8)
- Stryker.NET (verified: 8)
- diff-cover (verified: UNRESOLVED)
