FROM python:3.10.11

# switch working directory
WORKDIR /app

#copy all
COPY . /app

# install the dependencies and packages in the requirements file
RUN pip install -r requirements.txt

# install thuggingface-cli to download https://huggingface.co/radlab/polish-qa-v2
RUN huggingface-cli download radlab/polish-qa-v2

CMD [ "python", "./app.py"]