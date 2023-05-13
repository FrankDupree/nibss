 # Jenkins Base Image
FROM mcr.microsoft.com/dotnet/core/sdk:3.1
ENV WORKING_DIR=/usr/app/src/
ENV PATH="${PATH}:/root/.dotnet/tools"
ENV JAVA_HOME=/usr/lib/jvm/java-11-openjdk-amd64

WORKDIR ${WORKING_DIR}

RUN echo 'deb http://ppa.launchpad.net/ansible/ansible/ubuntu trusty main' >> /etc/apt/sources.list \
&& apt-key adv --keyserver keyserver.ubuntu.com --recv-keys 93C4A3FD7BB9C367 \
&& apt update \
&& apt install -y ansible sshpass default-jdk

RUN curl -sL https://deb.nodesource.com/setup_14.x | bash - && \
    apt-get install -y nodejs

RUN apt install -y python-pip && \
pip install docker

RUN apt-get install -y apt-transport-https ca-certificates curl unzip software-properties-common && \
    curl -fsSL https://download.docker.com/linux/debian/gpg | apt-key add - && \
    add-apt-repository \
       "deb [arch=amd64] https://download.docker.com/linux/debian \
       $(lsb_release -cs) \
       stable" && \
    apt update && \
    apt install -y docker-ce docker-ce-cli containerd.io

RUN dotnet tool install --global dotnet-sonarscanner && dotnet tool update -g Microsoft.Tye --version "0.4.0-alpha.20371.1"
RUN apt-get install -y python3-pip
RUN pip3 install dominate requests argparse

