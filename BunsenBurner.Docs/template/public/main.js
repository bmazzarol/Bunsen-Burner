/*
  Shared navigation icons for every bmazzarol docfx site.

  docfx does not emit an edit link for these sites, so the repository link is
  derived from the page itself: the "improve this doc" link when docfx does
  provide one, otherwise the first path segment of the GitHub Pages URL, which
  is the repository name.
*/

const org = 'bmazzarol'
const home = `https://${org}.github.io/bmazzarol/`

function repoUrl () {
  const url =
    document.querySelector('.edit-link')?.href ||
    document.querySelector('meta[name="docfx:docurl"]')?.content ||
    ''
  const match = url.match(/github\.com\/[^/]+\/[^/]+/)
  if (match) return 'https://' + match[0]

  const repo = location.pathname.split('/').filter(Boolean)[0] || 'bmazzarol'
  return `https://github.com/${org}/${repo}`
}

export default {
  iconLinks: [
    {
      icon: 'github',
      href: repoUrl(),
      title: 'Repository',
    },
    {
      icon: 'house',
      href: home,
      title: 'All projects',
    },
  ],
}
