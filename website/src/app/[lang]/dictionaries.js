import 'server-only'

const dictionaries = {
    ru: () => import( '/public/dictionaries/ru.json').then((module)=>module.default),
    en: () => import( '/public/dictionaries/en.json').then((module)=>module.default),
}
export const getDictionary = async (lang) => dictionaries[lang]();