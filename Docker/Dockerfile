FROM mcr.microsoft.com/dotnet/sdk:6.0
LABEL author="maciej.wozniczka@tpa-group.pl"

RUN apt-get update
RUN apt-get install -y curl
RUN apt-get install -y libpng-dev libjpeg-dev curl libxi6 build-essential libgl1-mesa-glx
RUN curl -sL https://deb.nodesource.com/setup_lts.x | bash -
RUN apt-get install -y nodejs

COPY . /app
WORKDIR /app
RUN dotnet build -c Release
RUN dotnet publish Syncfusion_HtmlToPdf -c Release

WORKDIR /app
RUN mkdir -p Syncfusion_HtmlToPdf/bin/Release/net6.0/publish/ClientApp
RUN mkdir -p Syncfusion_HtmlToPdf/bin/Release/net6.0/publish/ClientApp/dist

WORKDIR /app/Syncfusion_HtmlToPdf/bin/Release/net6.0/publish
CMD ./Syncfusion_HtmlToPdf