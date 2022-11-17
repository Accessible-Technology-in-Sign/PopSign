import os
import random
import json
#
# DO NOT EDIT THESE
#
word_banks = {}
banks_for_words = {}
frames_for_words = {}
words = []
word_groups = []
words_per_level = 5

#
#     EDIT BELOW
#
highest_level = 24
num_levels_to_create = 533
word_bank_names = ['ActionWords', 'DescriptiveWords', 'FoodAndDrink', 'Games', 'HelpingVerbs',
                   'Household', 'OutsideThings', 'People', 'PrepositionsAndPronouns', 'QuestionWords', 'Time', 'Vehicles']
video_connection_path = 'Assets/PopSignMain/Resources/VideoConnection/'
level_layout_path = 'Assets/PopSignMain/Resources/Levels/'
word_banks_path = 'Assets/PopSignMain/Resources/WordBanks/'
used_words = [
    'please', 'thank you', 'dad', 'mom', 'help',
    'brother', 'sister', 'love', 'hello', 'goodbye',
    'hungry', 'thirsty', 'sleepy', 'happy', 'sad',
    'my', 'you', 'yes', 'no', 'careful',
    'who', 'what', 'when', 'where', 'why']  # these are the words used in the first 24 levels of game


def load_words():
    count = 0
    tempList = []
    with open('Assets/PopSignMain/Resources/words.json', 'r') as f:  # open main word bank
        data = json.load(f)
        for word in data:
            if word not in used_words:
                words.append(word)
                banks_for_words[word] = data[word]['folderName']
                frames_for_words[word] = str(data[word]['frameNumber'])
                tempList.append(word)
                count += 1
                if count == 5:
                    count = 0
                    word_groups.append(tempList)
                    tempList = []


def create_file_from_words(level_words, level_number):
    new_video_connection_name = 'level' + str(level_number) + '.txt'
    # naming the example level files
    new_level_name = str(level_number) + '.txt'
    # Resources/VideoConnection/level{}.txt
    new_video_path = video_connection_path + new_video_connection_name
    new_level_path = level_layout_path + new_level_name
    with open(new_video_path, 'w+') as f:
        f.write('{\n')
        f.write('\t"bluefileName":"' +
                level_words[0] + '",\n')
        f.write('\t"blueframeNumber":' +
                frames_for_words[level_words[0]] + ',\n')
        f.write('\t"bluefolderName":"MacarthurBates/AllWords/' +
                banks_for_words[level_words[0]] + '/' + level_words[0] + '",\n')
        f.write('\t"blueImageName":"WordIcons/' + level_words[0] + '",\n')
        f.write('\t"greenfileName":"' + level_words[1] + '",\n')
        f.write('\t"greenframeNumber":' +
                frames_for_words[level_words[1]] + ',\n')
        f.write('\t"greenfolderName":"MacarthurBates/AllWords/' +
                banks_for_words[level_words[1]] + '/' + level_words[1] + '",\n')
        f.write('\t"greenImageName":"WordIcons/' + level_words[1] + '",\n')
        f.write('\t"redfileName":"' + level_words[2] + '",\n')
        f.write('\t"redframeNumber":' +
                frames_for_words[level_words[2]] + ',\n')
        f.write('\t"redfolderName":"MacarthurBates/AllWords/' +
                banks_for_words[level_words[2]] + '/' + level_words[2] + '",\n')
        f.write('\t"redImageName":"WordIcons/' + level_words[2] + '",\n')
        f.write('\t"violetfileName":"' + level_words[3] + '",\n')
        f.write('\t"violetframeNumber":' +
                frames_for_words[level_words[3]] + ',\n')
        f.write('\t"violetfolderName":"MacarthurBates/AllWords/' +
                banks_for_words[level_words[3]] + '/' + level_words[3] + '",\n')
        f.write('\t"violetImageName":"WordIcons/' + level_words[3] + '",\n')
        f.write('\t"yellowfileName":"' + level_words[4] + '",\n')
        f.write('\t"yellowframeNumber":' +
                frames_for_words[level_words[4]] + ',\n')
        f.write('\t"yellowfolderName":"MacarthurBates/AllWords/' +
                banks_for_words[level_words[4]] + '/' + level_words[4] + '",\n')
        f.write('\t"yellowImageName":"WordIcons/' + level_words[4] + '"\n')
        f.write('}\n')

        # borrow a previous level's structure and randomize it a little to make a new level
    random_level_number = random.randint(1, highest_level)
    random_level_name = str(random_level_number) + \
        '.txt'
    random_level_path = level_layout_path + \
        random_level_name  # Resources/Levels/{}.txt

    with open(new_level_path, 'w+') as f:
        with open(random_level_path, 'r+') as g:
            for line in g.readlines():
                if 'MODE' in line:
                    current_mode = line.split(' ')[1]
                elif 'SIZE' in line:
                    current_size = line.split(' ')[1].split('/')[1]
                elif 'COLOR LIMIT' in line:
                    color_limit = line.split(' ')[2]
                elif 'LIMIT' in line:
                    current_limit = line.split(' ')[1].split('/')[1]
                elif 'STARS' in line:
                    stars = line.split(' ')[1]
                    first_star = stars.split('/')[0]
                    second_star = stars.split('/')[1]
                    third_star = stars.split('/')[2]
                else:
                    game_line = line
                f.write(line)


load_words()
for i in range(highest_level, highest_level + num_levels_to_create):
    # -24 because word list does not contain words used in the first 24 levels. See used_words above
    level_words = word_groups[(i - 24) // 5]
    create_file_from_words(level_words, i)
