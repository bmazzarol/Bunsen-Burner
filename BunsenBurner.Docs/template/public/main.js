/*
  Shared navigation icons for every bmazzarol docfx site.

  The repository link is derived from the "improve this doc" link docfx already
  emits, so one file works for every project without hard-coding a repo name.
*/

const home = 'https://bmazzarol.github.io/'

function repoUrl () {
  const url =
    document.querySelector('.edit-link')?.href ||
    document.querySelector('meta[name="docfx:docurl"]')?.content ||
    ''
  const match = url.match(/github\.com\/[^/]+\/[^/]+/)
  return match ? 'https://' + match[0] : null
}

export default {
  iconLinks: [
    {
      icon: 'github',
      href: repoUrl() || 'https://github.com/bmazzarol',
      title: 'Repository',
    },
    {
      icon: 'house',
      href: home,
      title: 'All projects',
    },
  ],
}
