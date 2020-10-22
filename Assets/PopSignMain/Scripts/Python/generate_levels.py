import os
import random

video_frames_path = '../FramesToVideo/Resources/MacarthurBates/'
video_connection_path = 'Resources/VideoConnection/'
level_layout_path = 'Resources/Levels/'
word_banks_path = 'Resources/WordBanks/'
word_bank_names = ['DescriptiveWords', 'FoodAndDrink', 'OutsideThings', 'Clothing',
'ConnectingWords', 'PrepositionsAndPronouns', 'PlacesToGo', 'Time', 'QuestionWords',
'QuantifiersAndArticles', 'Animals', 'People', 'HelpingVerbs', 'Vehicles', 'Household',
'ActionWords', 'School', 'Games', 'SmallObjects']
word_groups = [
['please', 'thank you', 'dad', 'mom', 'help'],
['brother', 'sister', 'love', 'hello', 'goodbye'],
['hungry', 'thirsty', 'sleepy', 'happy', 'sad'],
['my', 'you', 'yes', 'no', 'careful'],
['who', 'what', 'when', 'where', 'why'],
['listen', 'look', 'sit', 'go', 'stop'],
['scared', 'mad', 'hurt', 'dirty', 'sick'],
['water', 'juice', 'milk', 'want', 'yucky'],
['breakfast', 'lunch', 'dinner' 'food', 'snack'],
['boy', 'girl', 'friend', 'play', 'toy'],
['now', 'tomorrow', 'yesterday', 'today', 'tonight'],
['apple', 'banana', 'orange', 'grape', 'strawberry'],
['sunny', 'rain', 'snow', 'hot', 'cold'],
['red', 'yellow', 'green', 'blue', 'black'],
['grandma', 'grandpa', 'hug', 'aunt', 'uncle'],
['car', 'bicycle', 'airplane', 'train', 'motorcycle']
]
word_banks = {}
banks_for_words = {}
frames_for_words = {}
words = []
already_used = []

num_levels_to_create = 80
words_per_level = 5
highest_level = 0

def generate_random_level(level_number):
    global words
    global already_used
    level_words = []
    for i in range(words_per_level):
        try:
            word = random.choice(words)
        except IndexError:
            words = already_used
            already_used = []
            word = random.choice(words)
        words.remove(word)
        already_used.append(word)
        level_words.append(word)
    return level_words

# we need to create two files: one representing the level structure itself for Resources/Levels
# and one for Resources/VideoConnection that defines what word corresponds to what color bubble
# in addition, the VideoConnection file will tell us where to find each word's video
def create_file_from_words(level_words, level_number):
    new_video_connection_name = 'level' + str(level_number) + '.txt'
    new_level_name = str(level_number) + '.txt'
    new_video_path = video_connection_path + new_video_connection_name
    new_level_path = level_layout_path + new_level_name

    with open(new_video_path, 'w+') as f:
        f.write('{\n')
        f.write('\t"bluefileName":"' + level_words[0] + '",\n')
        f.write('\t"blueframeNumber":' + frames_for_words[level_words[0]] + ',\n')
        f.write('\t"bluefolderName":"MacarthurBates/' + banks_for_words[level_words[0]] + '/' + level_words[0] + '",\n')
        f.write('\t"blueImageName":"WordIcons/' + level_words[0] + '",\n')
        f.write('\t"greenfileName":"' + level_words[1] + '",\n')
        f.write('\t"greenframeNumber":' + frames_for_words[level_words[1]] + ',\n')
        f.write('\t"greenfolderName":"MacarthurBates/' + banks_for_words[level_words[1]] + '/' + level_words[1] + '",\n')
        f.write('\t"greenImageName":"WordIcons/' + level_words[1] + '",\n')
        f.write('\t"redfileName":"' + level_words[2] + '",\n')
        f.write('\t"redframeNumber":' + frames_for_words[level_words[2]] + ',\n')
        f.write('\t"redfolderName":"MacarthurBates/' + banks_for_words[level_words[2]] + '/' + level_words[2] + '",\n')
        f.write('\t"redImageName":"WordIcons/' + level_words[2] + '",\n')
        f.write('\t"violetfileName":"' + level_words[3] + '",\n')
        f.write('\t"violetframeNumber":' + frames_for_words[level_words[3]] + ',\n')
        f.write('\t"violetfolderName":"MacarthurBates/' + banks_for_words[level_words[3]] + '/' + level_words[3] + '",\n')
        f.write('\t"violetImageName":"WordIcons/' + level_words[3] + '",\n')
        f.write('\t"yellowfileName":"' + level_words[4] + '",\n')
        f.write('\t"yellowframeNumber":' + frames_for_words[level_words[4]] + ',\n')
        f.write('\t"yellowfolderName":"MacarthurBates/' + banks_for_words[level_words[4]] + '/' + level_words[4] + '",\n')
        f.write('\t"yellowImageName":"WordIcons/' + level_words[4] + '"\n')
        f.write('}\n')
    '''
    # borrow a previous level's structure and randomize it a little to make a new level
    random_level_number = random.randint(1, highest_level)
    random_level_name = str(random_level_number) + '.txt'
    random_level_path = level_layout_path + random_level_name

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
    '''

def generate_from_word_bank(bank_name):
    category_bank = load_words(bank_name)
    level_words = []
    for i in range(words_per_level):
        word = random.choice(category_bank)
        level_words.append(word)
    return level_words

def find_highest_level():
    current_highest = 0
    all_names = os.listdir(level_layout_path)
    for name in all_names:
        if '.meta' in name or '.DS_Store' in name:
            pass
        else:
            name = name.split('level')[0]
            level_number = int(name.split('.txt')[0])
            if level_number > current_highest:
                current_highest = level_number
    return current_highest

def load_words(bank_name):
    with open(word_banks_path + bank_name + 'Bank.txt', 'r') as f:
        for line in f.readlines():
            word = line.strip()
            words.append(word)
            banks_for_words[word] = bank_name
            path_to_frames = '../FramesToVideo/Resources/MacarthurBates/' + bank_name + '/' + word
            num_frames = 0
            files = os.listdir(path_to_frames)
            for file in files:
                try:
                    number = int(file.split('.')[0][len(word):])
                    if(number > num_frames):
                        num_frames = number
                except (IndexError, ValueError):
                    pass
            frames_for_words[word] = str(num_frames)


# step 1: load in all words
for bank in word_bank_names:
    load_words(bank)
# step 2: find the highest level number (so we know what to name our new levels)
highest_level = 1
# step 3: create num_levels_to_create new level files with random words
for i in range(highest_level, highest_level + num_levels_to_create):
    # level_words = generate_random_level(i)
    level_words = word_groups[i / 5]
    create_file_from_words(level_words, i)
