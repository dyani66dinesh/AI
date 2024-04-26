import os.path
from pprint import pprint
import time
from io import BytesIO
from random import random
import uuid

from azure.cognitiveservices.vision.contentmoderator import ContentModeratorClient
import azure.cognitiveservices.vision.contentmoderator.models
from msrest.authentication import CognitiveServicesCredentials
#Screen needs to be imported here
from azure.cognitiveservices.vision.contentmoderator.models import Screen


CONTENT_MODERATOR_ENDPOINT = "CONTENT_MODERATOR_ENDPOINT"
subscription_key = "CONTENT_MODERATOR_SUBSCRIPTION_KEY"

client = ContentModeratorClient(
    endpoint=CONTENT_MODERATOR_ENDPOINT,
    credentials=CognitiveServicesCredentials(subscription_key)
)

TEXT_FOLDER = os.path.join(os.path.dirname(
    os.path.realpath(__file__)), ".")
    
# Screen the input text: check for profanity,
# do autocorrect text, and check for personally identifying
# information (PII)
#place the content_moderator_text_moderation.txt file in the root folder where your .py file is located

with open(os.path.join(TEXT_FOLDER, 'content_moderator_text_moderation.txt'), "rb") as text_fd:
    screen = client.text_moderation.screen_text(
        text_content_type="text/plain",
        text_content=text_fd,
        language="eng",
        autocorrect=True,
        pii=True
    )
    assert isinstance(screen, Screen)
    pprint(screen.as_dict())
    