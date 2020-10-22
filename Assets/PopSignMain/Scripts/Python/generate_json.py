import json
import os

word_banks_path = os.path.dirname(os.path.abspath(__file__)) + '/BubbleShooterEasterBunny/Resources/WordBanks'
video_frames_path = 'FramesToVideo/Resources/MacarthurBates'
word_bank_names = ['DescriptiveWords', 'FoodAndDrink', 'OutsideThings', 'Clothing',
'ConnectingWords', 'PrepositionsAndPronouns', 'PlacesToGo', 'Time', 'QuestionWords',
'QuantifiersAndArticles', 'Animals', 'People', 'HelpingVerbs', 'Vehicles', 'Household',
'ActionWords', 'School', 'Games', 'SmallObjects']
words = []
word_banks = {}
word_data = {}

# collect all words
def load_words(bank_name):
    with open(word_banks_path + '/' + bank_name + 'Bank.txt', 'r') as f:
        for line in f.readlines():
            word = line.strip()
            words.append(word)
            word_banks[word] = bank_name

for bank in word_bank_names:
    load_words(bank)

for word in words:
    try:
        # connect words to their respective folders
        folder_path = os.path.dirname(os.path.abspath(__file__)) + '/FramesToVideo/Resources/MacarthurBates/' + word_banks[word] + '/' + word
        # count the number of frames in each word's video
        frame_names = [int(f.split(word)[1].strip('.jpg')) for f in os.listdir(folder_path) if f.endswith('.jpg')]
        frame_names.sort()
        # print(word)
        # print(frame_names)
        max_frame_number = frame_names[-1]
        # print(max_frame_number)
        word_data[word] = {'folderName' : word_banks[word], 'frameNumber' : max_frame_number}
    except (FileNotFoundError, IndexError):
        print(word)

# add entry to json
with open('words.json', 'w+') as f:
    f.write(json.dumps(word_data, indent=4))
