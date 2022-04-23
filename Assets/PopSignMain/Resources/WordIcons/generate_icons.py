from PIL import Image, ImageFont, ImageDraw
import os

font = ImageFont.truetype('MagallanesCondLight.otf', size=130)
color = 'rgb(255, 255, 255)'
size = (320, 320)
words = ['day', 'dinosaur', 'doll', 'glass,cup', 'cook']

with open('FullWordBank.txt', 'r+') as f:
    for line in f.readlines():
        # get word from the line
        word = line.strip()
        # calculate the size of the word
        word_size = font.getsize(word)
        # if the word size will exceed the dimensions of the image, change the font size
        font = ImageFont.truetype('MagallanesCondLight.otf', size=120)
        word_size = font.getsize(word)
        if len(word) > 3:
            font = ImageFont.truetype('MagallanesCondLight.otf', size=110)
            word_size = font.getsize(word)
            if len(word) > 5:
                font = ImageFont.truetype('MagallanesCondLight.otf', size=95)
                word_size = font.getsize(word)
                if len(word) > 7:
                    font = ImageFont.truetype('MagallanesCondLight.otf', size=60)
                    word_size = font.getsize(word)
        # calculate where the image should be drawn
        image_x = 160 - word_size[0] / 2
        image_y = 160 - word_size[1] / 2
        # create and open a new image with no content in it
        image = Image.new('RGBA', size, 'rgba(255, 255, 255, 0)')
        # get a draw object for the new image
        draw = ImageDraw.Draw(image)
        # draw the word (line) on the background
        draw.text((image_x, image_y), word, fill=color, font=font)
        # if the file already exists, delete it and replace it with a new one
        if os.path.exists(word + '.png'):
            os.remove(word + '.png')
        # save the edited image
        image.save(word + '.png', 'PNG')
