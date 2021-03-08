# FileUploader

File upload scenarios with asp.net core. 

## Upload file using a `byte []` embedded in a model

Using the `byte []` embedded in a model will force the uplaod in JSON and the file is therefore obliged to be 
encoded as base64, thus increasing the request payload size by about 30%.

### Advantages

- Pure JSON payloads
- Simpler C# model (depends only on base CLR types)

### Disadvantages

- Larger payload due to base64 encoding obligation
- Impractical with swagger

## Upload file using `IFormFile` embedded in a model

Using the `IFormFile` avoid this but the payload has to be posted as a `multipart/form-data`. You could therefore
argue that this is less RESTful but it's certainly faster and has the advantage of encapsulating the filename/mimetype
in the object. It also plays nicely with the latest versions of swagger UI (swashbuckle).

### Advantages

- Smaller payloads
- `IFormFile` object encapsulates useful properties: filename, mimetype etc.
- Plays nice with swagger ui

### Disadvantages

- Less RESTful (not JSON)
- Dependeny on non base CLR type (`IFormFile`).
