/*
  Shared navigation icons and accent colour for every bmazzarol docfx site.

  The repository link for the nav icons is taken from docfx's own "Edit this
  page" link when it is present, otherwise from the first path segment of the
  GitHub Pages URL, which is the repository name.

  The accent colour is sampled from the project's own icon, so each site's
  links, focus rings and text selection carry its logo.
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

/* ---------------------------------------------------------------- accent */

const minContrast = 4.5
const lightBackground = [255, 255, 255]
const darkBackground = [33, 37, 41]

function clamp (value, min, max) {
  return Math.min(max, Math.max(min, value))
}

function hslToRgb (hue, saturation, lightness) {
  const c = (1 - Math.abs(2 * lightness - 1)) * saturation
  const x = c * (1 - Math.abs(((hue / 60) % 2) - 1))
  const m = lightness - c / 2
  const [r, g, b] =
    hue < 60 ? [c, x, 0] :
    hue < 120 ? [x, c, 0] :
    hue < 180 ? [0, c, x] :
    hue < 240 ? [0, x, c] :
    hue < 300 ? [x, 0, c] : [c, 0, x]
  return [
    Math.round((r + m) * 255),
    Math.round((g + m) * 255),
    Math.round((b + m) * 255),
  ]
}

function luminance (rgb) {
  return rgb
    .map((channel) => {
      const c = channel / 255
      return c <= 0.03928 ? c / 12.92 : Math.pow((c + 0.055) / 1.055, 2.4)
    })
    .reduce((total, channel, index) => total + [0.2126, 0.7152, 0.0722][index] * channel, 0)
}

function contrast (rgb, background) {
  const a = luminance(rgb)
  const b = luminance(background)
  return (Math.max(a, b) + 0.05) / (Math.min(a, b) + 0.05)
}

/* The dominant hue of a rasterised image: every pixel that carries colour is
   counted in a 36 bin hue histogram, weighted by how saturated and how bright
   it is. Greys, white and black are ignored, they have no hue to offer. */
function dominantHue (image) {
  const size = 64
  const canvas = document.createElement('canvas')
  canvas.width = canvas.height = size
  const ctx = canvas.getContext('2d')
  ctx.drawImage(image, 0, 0, size, size)
  const pixels = ctx.getImageData(0, 0, size, size).data

  const weight = new Array(36).fill(0)
  const saturation = new Array(36).fill(0)

  for (let i = 0; i < pixels.length; i += 4) {
    const r = pixels[i] / 255
    const g = pixels[i + 1] / 255
    const b = pixels[i + 2] / 255
    const a = pixels[i + 3] / 255
    if (a < 0.5) continue

    const max = Math.max(r, g, b)
    const min = Math.min(r, g, b)
    const chroma = max === 0 ? 0 : (max - min) / max
    if (chroma < 0.2 || max < 0.15 || max > 0.95) continue

    const delta = max - min
    let hue
    if (max === r) hue = ((g - b) / delta) % 6
    else if (max === g) hue = (b - r) / delta + 2
    else hue = (r - g) / delta + 4
    hue = (hue * 60 + 360) % 360

    const bin = Math.floor(hue / 10) % 36
    weight[bin] += chroma * max
    saturation[bin] += chroma
  }

  let best = 0
  for (let i = 1; i < weight.length; i++) {
    if (weight[i] > weight[best]) best = i
  }
  if (!weight[best]) return null

  return {
    hue: best * 10 + 5,
    saturation: clamp(saturation[best] / weight[best], 0.45, 0.7),
  }
}

/* The most vivid tone of that hue that still reads as text: as light as
   possible against the light theme, as dark as possible against the dark one,
   both held at a 4.5:1 contrast ratio. */
function readableAccent (hue, saturation, background, preferLight) {
  const stops = preferLight
    ? [0.75, 0.7, 0.65, 0.6, 0.55, 0.5, 0.45, 0.4, 0.35, 0.3]
    : [0.3, 0.35, 0.4, 0.45, 0.5, 0.55, 0.6, 0.65, 0.7, 0.75]
  for (const lightness of stops) {
    if (contrast(hslToRgb(hue, saturation, lightness), background) >= minContrast) {
      return lightness
    }
  }
  return 0.46
}

function applyAccent (image) {
  let colour
  try {
    colour = dominantHue(image)
  } catch (error) {
    /* a cross origin logo taints the canvas; keep the fallback colour */
    return
  }
  if (!colour) return

  const light = readableAccent(colour.hue, colour.saturation, lightBackground, true)
  const dark = readableAccent(colour.hue, colour.saturation, darkBackground, false)
  const root = document.documentElement
  const saturation = Math.round(colour.saturation * 100)
  root.style.setProperty(
    '--bm-accent-light',
    `hsl(${colour.hue} ${saturation}% ${Math.round(light * 100)}%`
  )
  root.style.setProperty(
    '--bm-accent-dark',
    `hsl(${colour.hue} ${saturation}% ${Math.round(dark * 100)}%`
  )
}

const logo = document.getElementById('logo')
if (logo) {
  if (logo.complete) applyAccent(logo)
  else logo.addEventListener('load', () => applyAccent(logo), { once: true })
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
