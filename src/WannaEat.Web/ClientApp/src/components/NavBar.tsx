import React from 'react';

const NavBar = () => {
    return (
        <>
            <div style={{
                backgroundColor: 'red'
            }} className={'h-auto'}>
                
            <div className={'container-lg container-fluid shadow bg-red'}>
                <nav className={'navbar navbar-expand-lg navbar-dark text-white-50'}>
                    <a className={'navbar-brand'} href={'#'}>WannaEat</a>
                </nav>
            </div>
            </div>
        </>
    );
};

export default NavBar;