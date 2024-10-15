// Import do Tailwind e a construção da página
const addTailwindCSS = () => {
    const tailwind = document.createElement('script');
    tailwind.src = 'https://cdn.tailwindcss.com';
    tailwind.onload = () => {
        console.log('Tailwind CSS carregado.');
        document.body.classList.add("h-screen", "w-screen", "bg-gray-800");
        applyTailwindStyles();
    };

    tailwind.onerror = () => {
        console.error('Falha ao carregar Tailwind CSS.');
    };

    document.head.appendChild(tailwind);
};

const applyTailwindStyles = () => {
    document.body.classList.add('font-sans', 'm-0');

    // Adiciona o estilo do scrollbar
    const styleContent = `
        /* Estilo do scrollbar */
        ::-webkit-scrollbar {
            width: 12px;
        }

        ::-webkit-scrollbar-track {
            background: #2d3748;
            border-top-left-radius: 6px;
            border-top-right-radius: 6px;
        }

        ::-webkit-scrollbar-thumb {
            background-color: #4a5568;
            border-radius: 6px;
            border: 3px solid #2d3748;
        }

        ::-webkit-scrollbar-thumb:hover {
            background-color: #718096;
        }`;

    const styleElement = document.createElement('style');
    styleElement.textContent = styleContent;
    document.head.appendChild(styleElement);

    renderBodyContent();
};

const renderBodyContent = () => {
    document.body.appendChild(createNav());
    document.body.appendChild(createMainContainer());
};

const createNav = () => {
    const nav = document.createElement('nav');
    nav.classList.add('flex', 'h-[5vh]', 'shadow-sm', 'bg-gray-900');

    const titleDiv = document.createElement('div');
    titleDiv.classList.add('flex', 'justify-center', 'items-center', 'p-2');
    const title = document.createElement('h1');
    title.classList.add('font-bold', 'text-xl', 'text-white', 'p-2');
    title.textContent = 'Docsnap';
    titleDiv.appendChild(title);

    const versionDiv = document.createElement('div');
    versionDiv.classList.add('flex', 'justify-center', 'items-center', 'bg-red-500', 'rounded-md', 'my-3', 'mx-1');
    const versionText = document.createElement('p');
    versionText.classList.add('text-white', 'p-2');
    versionText.textContent = 'v0.1.3';
    versionDiv.appendChild(versionText);

    nav.appendChild(titleDiv);
    nav.appendChild(versionDiv);

    return nav;
};

const createMainContainer = () => {
    const container = document.createElement('div');
    container.classList.add('flex', 'h-[95vh]');

    const aside = createAside();
    const main = createMainContent();

    container.appendChild(aside);
    container.appendChild(main);

    console.log('Container principal criado.');
    return container;
};

const createAside = () => {
    const aside = document.createElement('aside');
    aside.classList.add('flex', 'flex-col', 'justify-between', 'w-3/12', 'bg-gray-900', 'shadow-md', 'overflow-auto');

    const select = createSelect();
    const lastSelectedDiv = createLastSelectedDiv();
    const sidebarMenu = createSidebarMenu();

    aside.appendChild(select);
    aside.appendChild(lastSelectedDiv);
    aside.appendChild(sidebarMenu);

    console.log('Aside criado.');
    return aside;
};

const createSelect = () => {
    const select = document.createElement('select');
    select.classList.add('w-[90%]', 'text-center', 'bg-transparent', 'text-white', 'h-9', 'shadow-md', 'm-2');
    const options = ['Modulo 1', 'Modulo 2', 'Modulo 3', 'Modulo 4', 'Modulo 5'];
    options.forEach(optionText => {
        const option = document.createElement('option');
        option.textContent = optionText;
        select.appendChild(option);
    });
    console.log('Select criado.');
    return select;
};

const createLastSelectedDiv = () => {
    const div = document.createElement('div');
    div.id = 'last-selecteds';
    div.classList.add('flex', 'flex-col', 'justify-start', 'items-center', 'h-full', 'w-full', 'overflow-y-auto');
    console.log('Últimos selecionados criado.');
    return div;
};

const createSidebarMenu = () => {
    const ul = document.createElement('ul');
    ul.classList.add('text-gray-500', 'pb-[15px]', 'pl-[15px]', 'flex', 'flex-col', 'gap-2');
    const items = ['Dashboard', 'Documents', 'Settings'];
    items.forEach(itemText => {
        const li = document.createElement('li');
        li.textContent = itemText;
        ul.appendChild(li);
    });
    console.log('Menu lateral criado.');
    return ul;
};

const createMainContent = () => {
    const main = document.createElement('main');
    main.classList.add('w-9/12', 'bg-gray-700', 'rounded-t-md', 'mr-2', 'p-5', 'overflow-y-auto');
    main.innerHTML = `
        <h1 class="text-white">Products</h1>
        <p class="text-gray-400">description</p>
        ${createActionItem('bg-lime-300', 'Create Product', 'POST')}
        ${createActionItem('bg-blue-300', 'Update Product', 'PATCH')}
        ${createActionItem('bg-red-300', 'Delete Product', 'DELETE')}
        ${createLoremContent()}
    `;
    console.log('Conteúdo principal criado.');
    return main;
};

const createActionItem = (color, text, method) => `
    <div class="shadow-lg rounded-md p-2 my-2 flex items-center border-2 border-gray-800"
        onclick="last_selected('${color}', '${text}', '${method}')">
        <div class="${color} w-20 rounded-md mr-3">
            <p class="text-center text-gray-900">${method}</p>
        </div>
        <p class="text-center text-gray-400">${text}</p>
    </div>
`;

const createLoremContent = () => `
    <p class="text-gray-400">Lorem ipsum dolor sit amet consectetur adipisicing elit...</p>
    <!-- Add more content as needed -->
`;

document.addEventListener('DOMContentLoaded', () => {
    console.log('DOM carregado.');
    addTailwindCSS();

    window.last_selected = function (color, text, method) {
        let last_selecteds = document.getElementById('last-selecteds');

        let exists = Array.from(last_selecteds.children).find(div =>
            div.innerHTML.includes(text) && div.innerHTML.includes(method)
        );

        if (exists) {
            last_selecteds.removeChild(exists);
        }

        let div = document.createElement('div');
        div.classList.add('shadow-lg', 'rounded-md', 'p-1', 'my-1', 'flex', 'items-center', 'border-2', 'border-gray-800', 'text-xs', 'w-[95%]');
        div.innerHTML = `
            <div class="${color} w-12 rounded-md mr-3">
                <p class="text-center text-gray-900">${method}</p>
            </div>
            <p class="text-center text-gray-400">${text}</p>
        `;
        last_selecteds.insertBefore(div, last_selecteds.firstChild);
    };
});