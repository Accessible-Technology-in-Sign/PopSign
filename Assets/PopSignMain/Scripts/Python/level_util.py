import random
import os


def parse_lines(lines):
    header = []
    game_lines = []
    for line in lines:
        if any(x in ['MODE', 'SIZE', 'COLOR LIMIT', 'LIMIT', 'STARS'] for x in line.split()):
            header.append(line)
        else:
            game_lines.append(line)
    return header, game_lines


def has_bubbles(game_line):
    if any(x in ['1', '2', '3', '4', '5', '6', '7'] for x in game_line.split()):
        return True
    return False


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


def has_empty_above(game_lines, i):
    if i > 0 and not has_bubbles(game_lines[i - 1]):
        return True
    return False


def has_empty_below(game_lines, i):
    if i < len(game_lines) - 1 and not has_bubbles(game_lines[i + 1]):
        return True
    return False


def find_borders(game_lines):
    """Finds the indices of the top-most line with bubbles and the bottom-most line with bubbles."""
    top_free_space, bottom_free_space = count_free_space(game_lines)
    top_border = top_free_space
    bottom_border = len(game_lines) - bottom_free_space - 1
    return top_border, bottom_border


def find_candidate(game_lines):
    """Finds a random candidate line to duplicate (has bubbles and has an empty line either above or below)."""
    indices = list(range(len(game_lines)))
    random.shuffle(indices)
    for i in indices:
        if has_bubbles(game_lines[i]) and has_empty_above(game_lines, i):
            return i

        if has_bubbles(game_lines[i]) and has_empty_below(game_lines, i):
            return i
    return -1


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


def insert_space(game_lines, i):
    """Inserts a blank line below line i and removes one of the 'free' spaces above or below existing bubbles."""
    if len(game_lines[0]) != 22:
        return
    if i < 0 or i >= len(game_lines):
        return
    
    # We don't want to make the file longer than it is, so make sure we can remove something from the top or bottom.
    top_space, bottom_space = count_free_space(game_lines)
    if top_space == 0 and bottom_space == 0:
        return

    blank_line = '0 0 0 0 0 0 0 0 0 0 0\n'
    game_lines.insert(i + 1, blank_line)

    if top_space > 0:
        del game_lines[0]
    elif bottom_space > 0:
        del game_lines[-1]
    

def insert_spaces(game_lines, num_lines):
    """Randomly inserts num_lines blank lines in between the lines of bubbles."""
    count = 0
    for i in range(num_lines):
        top_space, bottom_space = count_free_space(game_lines)
        if top_space == 0 and bottom_space == 0:
            break

        top_border, bottom_border = find_borders(game_lines)
        indices = list(range(top_border, bottom_border))

        insert_space(game_lines, random.choice(indices))
        count += 1
    return count


def generate_stretched_level(game_lines, num_lines):
    new_game_lines = game_lines.copy()
    insert_spaces(new_game_lines, num_lines)
    fill_gaps(new_game_lines)
    return new_game_lines


def change_limits(limit=60):
    """
    Changes the LIMIT value (ammo in the player's cannon) for each text file in
    the same directory as this file.

    To use, place this level_util.py file in the same folder as the text files for each
    level (as of this moment: \PopSign\Assets\PopSignMain\Resources\Levels), and call
    this function.
    
    TODO: Change to use folder path as an argument. Maybe base limit on number of bubbles?

    Parameters:
    limit: the amount of ammo to set for each file
    """
    count = 0
    for file in os.listdir():
        filename = os.fsdecode(file)
        if filename.endswith('.txt'):
            with open(filename, 'r+') as f:
                lines = f.readlines()
                header, game_lines = parse_lines(lines)
                new_header = []
                if len(header) == 0:
                    return
                for line in header:
                    if line[:5] == 'LIMIT':
                        numbers = line.split()[1].split('/')
                        numbers[1] = str(limit)
                        numbers_str = '/'.join(numbers)
                        new_line = 'LIMIT ' + numbers_str + '\n'
                        new_header.append(new_line)
                    else:
                        new_header.append(line)
                new_level = new_header + game_lines
            
            with open(filename, 'w+') as f:
                f.writelines(new_level)
                count += 1
    print("Files modified:", count)


def count_bubbles(game_lines):
    """Returns the count of bubbles in a level's lines."""
    bubbles = ['1', '2', '3', '4', '5', '6']
    count = 0
    for line in game_lines:
        for c in line:
            if c in bubbles:
                count += 1
    return count


def set_stars_values(points_per_bubble=50):
    """
    Changes the star values of each level's text file based on the
    number of bubbles in that level.

    The star/score system is currently as follows:
        1 star = roughly 1/3 of bubbles have been popped
        2 stars = roughly 2/3 of bubbles have been popped
        3 stars = almost all bubbles popped
    
    Because the bubbles that a player shoots out of a cannon contribute
    to the player's score, it is impossible to predict the precise amount
    of score needed to clear all bubbles. Instead, we make the conservative
    assumption that the user shoots approximately 1 bubble
    for every 7 bubbles in the level. Because this is an underestimate,
    the player will usually win before popping all of the bubbles.

    To use, place this level_util.py file in the same folder as the text files for each
    level (as of this moment: \PopSign\Assets\PopSignMain\Resources\Levels), and call
    this function.
    
    TODO: Change to use folder path as an argument.

    Parameters:
    points_per_bubble: the number of points a user gets for popping a bubble (currently 50).
    """
    count = 0
    for file in os.listdir():
        filename = os.fsdecode(file)
        if filename.endswith('.txt'):
            with open(filename, 'r+') as f:
                lines = f.readlines()
                header, game_lines = parse_lines(lines)
                new_header = []
                if len(header) < 5:
                    return
                for line in header:
                    if line[:5] == 'STARS':
                        # Set new values to earn each star based on number of bubbles in the level.
                        num_bubbles = count_bubbles(game_lines)
                        star1 = num_bubbles * points_per_bubble // 3  # Get 1 star after popping roughly 1/3 of bubbles
                        star2 = num_bubbles * points_per_bubble * 2 // 3  # Get 2 stars after popping roughly 2/3 of bubbles
                        star3 = num_bubbles * points_per_bubble  # Get 3 stars after popping roughly all bubbles
                        star3 += (num_bubbles // 7 * points_per_bubble)  # Assume the player shoots approx. 1 bubble for every 7bubbles in the level

                        new_line = f'STARS {star1}/{star2}/{star3}\n'
                        new_header.append(new_line)
                    else:
                        new_header.append(line)
                new_level = new_header + game_lines
            
            with open(filename, 'w+') as f:
                f.writelines(new_level)
                count += 1
    print("Files modified:", count)


# def main(input_file, output_file):
#     with open(input_file, 'r+') as f:
#         lines = f.readlines()
#         header, game_lines = parse_lines(lines)

#         # print(count_free_space(game_lines))
#         # print(find_candidate(game_lines))
#         # print(find_borders(game_lines))
#         # print(find_nonborder_candidate(game_lines))

#         fill_gaps(game_lines)
#         # new_game_lines = generate_stretched_level(game_lines, 5)
#         # print(lines)
#         # print('----------------------')
#         # print(header)
#         # print('----------------------')
#         # print(game_lines)
#         # print('----------------------')
#         # print(len(header))
#         # print(len(game_lines))

#         # new_level = header + new_game_lines
#         new_level = header + game_lines
    
#     with open(output_file, 'w+') as f:
#         f.writelines(new_level)

if __name__=="__main__":
    # main('test_level.txt', 'test_output.txt')
    # change_limits()
    # set_stars_values(points_per_bubble=50)
    pass