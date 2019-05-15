before setting appsettings.json
```
"JWT":{
  "SecretKey":"[your jwt key]"
}
```

## Build and run the sample with Docker for Linux containers
```console
cd rktkrjatordyd
docker build -t lyrics-api .
docker run -it --rm -p 8000:80 lyrics-api
```

## Build and run the sample locally
```console
cd rktkrjatordyd
dotnet build
dotnet run
```