$tagName = $env:assembly_version
Write-Host "Push tag "$tagName
git config --global credential.helper store
Add-Content "$HOME\.git-credentials" "https://$($env:access_token):x-oauth-basic@github.com`n"
git tag $tagName "$env:APPVEYOR_REPO_COMMIT"
git push origin $tagName
Write-Host "Tag "$tagName" was pushed"
