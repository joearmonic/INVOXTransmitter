# EXECUTING THE EXAMPLE

## Preconditions
- Visual Studio with Azure development Tools.
- Postman (could be the web application, requires a free account, or the desktop version).

## Preparing the example
First we need to import the postman collection from _.solutionItems_. 
- Open postman to your personal workspace and click on `Import` button besides your workspace's name to the left column.
- Select `INVOX.postman_collection.json`
- Open the request `https://localhost:5001/VoiceFiles/Upstream`

The needed files to run the example are in the _.solutionItems\mp3Samples_ (if you use Solution Explorer view) or _SolutionItems\mp3Samples_ folder in the project folder structure.
Those files must be uploaded to Postman (they will be stored locally in `C:\Users\{local user}\Postman\files`) following the [postman tutorial to upload files](!https://www.postman.com/postman/workspace/postman-answers/documentation/13455110-00378d5c-5b08-4813-98da-bc47a2e6021d).
Once done, we can run the example.
Prepare the VS Solution Startup by selecting _INVOXMedicalTranscriptor_, _INVOXReceptor_ and _INVOXTransmitter_ projects in Configure Startup Projects menu option.
CLick on _Start_ in Visual studio. The function will start listening for request and Azurite service (azure file and blob storage service) will start running as well as the Transcriptor and the Receptor Apis.
The location of the blob storage will be `C:\Users\{local user}\AppData\Local\.vstools\azurite`. It is recommended to download Microsoft Azure Storage Explorer to verify visually the outcome of the execution from [Microsoft Azure Storage Explorer](!https://github.com/Microsoft/AzureStorageExplorer/releases/tag/v1.32.1)

## Running the example.
- INVOXReceptor is a WebApi in charge of loading the recordings from the doctors in the Azure Storage. This is a mockup of whatever other piece of Software would be doing this.
- INVOXTranscriptor is the WebApi that receives mp3 validated files and transcripts them to text files returning the transcription to the Transmitter.
- INVOXTransmitter. This is the target of the Sample. it has a Domain driven design that encourage decoupling the implementation details of the delivery and external mechanism from the core business of the Application. It works as an Azure Function that executes every day at 21. (the sample is configured to run at startup and every 5 minutes to see the results in a debug environment).
Important to note is that this could be achieve using a Console, a Windows Service, webjob, etc. The important thing here is that Cloud services could help to scale among lots of clients in distributed regions, which makes this solution perfect for company wide extensibility.

From postman, we select body tab and select the files we want to store in the blob Storage Emulator as a result of doctor's recordings and we set the userName of the person that transcripts . (we can run the example changing the username in the body to emulate different users sending their files). And we click on send.
To check the result it's recommended to open Azure Storage Explorer's Emulator Storage Account to see that a container is created for the User and the relevant files are created as blobs.

If the function (IVOXTransmitter) has run, we only have to wait a couple of minutes to see how the files are downloaded an transmitted to the Transcriptor Service, that in turns will return back the file transcripted and it will be store as txt files.
The criteria for the function to select files is purely the creation time, this way, the service in standard conditions, will take all files from the previous day and send them only during that execution.




