# FileUploader

File upload scenarios with ASP.NET Core. 

## Upload file using a `byte []` embedded in a model

Using the `byte []` embedded in a model will force the upload in JSON and the file is therefore obliged to be 
encoded as base64, thus increasing the request payload size by about 30%.

### *Advantages*

- Pure JSON payloads (`Content-Type: application/json`)
- Simpler C# model (depends only on base CLR types)

### *Disadvantages*

- Larger payload due to base64 encoding obligation
- Impractical with swagger
- Have to add separate properties for metadata (filename, mimetype etc)

## Upload file using `IFormFile` embedded in a model

Using the `IFormFile` avoid this but the payload has to be posted as a `multipart/form-data`. You could therefore
argue that this is less RESTful but it's certainly faster and has the advantage of encapsulating the filename/mimetype
in the object. It also plays nicely with the latest versions of swagger UI (Swashbuckle).

### *Advantages*

- Smaller payloads
- `IFormFile` object encapsulates useful properties: filename, mimetype etc
- Plays nicely with swagger UI

### *Disadvantages*

- Less RESTful (`Content-Type: multipart/form-data`)
- Dependency on non base CLR type (`IFormFile`)

## Security considerations

Two very good articles which talk about the security considerations when uploading files.

- [OWASP, Unrestricted file upload](https://owasp.org/www-community/vulnerabilities/Unrestricted_File_Upload)
- [Uploading  files in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-5.0)
