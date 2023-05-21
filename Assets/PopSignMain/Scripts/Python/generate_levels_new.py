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
highest_level = 160
num_levels_to_create = 500
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
        tabulatedLevel = []
        with open(random_level_path, 'r+') as g:
            for line in g.readlines():
                
                if 'MODE' in line or 'SIZE' in line or 'COLOR LIMIT' in line or 'LIMIT' in line or 'STARS' in line:
                    #leave settings as is.
                    f.write(line)
                else:
                    line_arr = []
                    offset_amount = 0
                    count = 0
                    #push all 6s into array
                    if '6' in line or '7' in line:
                        for char in line:
                            if char != ' ' and char != '\n':
                                line_arr.append(char)
                        #offset every two lines by the same amount
                        if count % 2 == 0:
                            offset_amount = random.randint(1, 9)
                        new_arr = [None] * 11
                        for i in range(len(line_arr)):
                            new_arr[(i+offset_amount) % 11] = line_arr[i]

                        tabulatedLevel.append(new_arr)
                        count += 1
                    elif '0' in line:
                        for char in line:
                            if char != ' ' and char != '\n':
                                line_arr.append(char)
                        tabulatedLevel.append(line_arr)
                #f.write(line)
        #go through f, look for and remove small islands, modifying star totals
        game_lines = []
        
        for row in range(len(tabulatedLevel)):
            for col in range(len(tabulatedLevel[0])):
                     if (checkSmallIslands(tabulatedLevel, row, col)):
                         tabulatedLevel[row][col] = '0'
                        
            new_line = " ".join(tabulatedLevel[row])
            new_line = new_line.lstrip(" ").rstrip(" ") + '\n'
            game_lines.append(new_line)
                    
            #f.write(new_line)
        #find and fill gaps in the level.
        fill_gaps(game_lines)
        for line in game_lines:
            f.write(line)
        #add 0s to remaining lines
        # for _ in range(35 - len(tabulatedLevel) - 6):
        #     f.write('0 0 0 0 0 0 0 0 0 0 0' + '\n')
            
        # f.write('0 0 0 0 0 0 0 0 0 0 0')

def checkSmallIslands(level_layout, x, y):
    
    if  x < 0 or y < 0 or x >= len(level_layout) or y >= len(level_layout[0]):
        return False
    if level_layout[x][y] == '0':
        return False
    island_size_threshold = 5
    visited = [[False for i in range(len(level_layout[0]))] for j in range(len(level_layout))]
    return countIslandSize(level_layout, x, y, visited) < island_size_threshold

def countIslandSize(level_layout, x, y, visited):
    if  x < 0 or y < 0 or x >= len(level_layout) or y >= len(level_layout[0]) or level_layout[x][y] == '0' or visited[x][y]:
        return 0
    else:
        visited[x][y] = True
        return 1 + countIslandSize(level_layout, x + 1, y, visited) + countIslandSize(level_layout, x, y+1, visited) + countIslandSize(level_layout, x, y - 1, visited) + countIslandSize(level_layout, x - 1, y, visited)


def find_nonborder_candidate(game_lines):
    """Finds a random candidate line to duplicate (has bubbles and has an empty line either above or below) that is not a border."""
    indices = list(range(len(game_lines)))
    random.shuffle(indices)
    for i in indices:
        top_border, bottom_border = find_borders(game_lines)
        if i != top_border and i != bottom_border:
            if has_bubbles(game_lines[i]) and has_empty_above(game_lines, i):
                return i

            if has_bubbles(game_lines[i]) and has_empty_below(game_lines, i):
                return i
    return -1


def fill_above(game_lines, i):
    """Fill in the empty line above by duplicating the candidate line"""
    if i > 0:
        game_lines[i - 1] = game_lines[i]


def fill_below(game_lines, i):
    """Fill in the empty line below by duplicating the candidate line"""
    if i < len(game_lines) - 1:
        game_lines[i + 1] = game_lines[i]

def find_borders(game_lines):
    """Finds the indices of the top-most line with bubbles and the bottom-most line with bubbles."""
    top_free_space, bottom_free_space = count_free_space(game_lines)
    top_border = top_free_space
    bottom_border = len(game_lines) - bottom_free_space - 1
    return top_border, bottom_border

def count_free_space(game_lines):
    top_free_space = 0
    bottom_free_space = 0

    for game_line in game_lines:
        if has_bubbles(game_line):
            break
        else:
            top_free_space += 1
    
    for game_line in reversed(game_lines):
        if has_bubbles(game_line):
            break
        else:
            bottom_free_space += 1
    
    return (top_free_space, bottom_free_space)

def has_bubbles(game_line):
    if any(x in ['1', '2', '3', '4', '5', '6', '7'] for x in game_line.split()):
        return True
    return False

def fill_gaps(game_lines):
    """Finds any 'gaps' within the level and fills them using lines above or below that are not empty"""
    candidate_idx = find_nonborder_candidate(game_lines)
    while candidate_idx != -1:
        if has_empty_above(game_lines, candidate_idx) and has_empty_below(game_lines, candidate_idx):
            r = random.randrange(0, 2)
            if r == 0:
                fill_above(game_lines, candidate_idx)
            else:
                fill_below(game_lines, candidate_idx)
        elif has_empty_above(game_lines, candidate_idx):
            fill_above(game_lines, candidate_idx)
        elif has_empty_below(game_lines, candidate_idx):
            fill_below(game_lines, candidate_idx)
        
        candidate_idx = find_nonborder_candidate(game_lines)

def has_empty_below(game_lines, i):
    if i < len(game_lines) - 1 and not has_bubbles(game_lines[i + 1]):
        return True
    return False

def has_empty_above(game_lines, i):
    if i > 0 and not has_bubbles(game_lines[i - 1]):
        return True
    return False


load_words()
for i in range(highest_level, highest_level + num_levels_to_create):
    # -24 because word list does not contain words used in the first 27 levels. See used_words above
    level_words = word_groups[(i - 27) // 5]
    create_file_from_words(level_words, i)
