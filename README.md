# CellMates

Contributing

The master branch is for stable builds only, please branch off of develop. 
If you have any current branches directly off of master, here's how to change it:

https://gist.github.com/iolloyd/4018744c054b223c035ed8233f1ea6ce <br />

i.e.
if you currently have master/my-branch and you want it to be develop/my-branch:

git checkout develop

git checkout -b new-branch

git rebase --onto new-branch master my-branch


