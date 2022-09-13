import React from 'react';

const NavBar = () => {
    return (
        <div className={'h-auto bg-primary shadow'}>
            <div className={'container-lg container-fluid bg-red'}>
                <nav className={'navbar navbar-expand-lg navbar-dark text-white-50'}>
                    <a className={'navbar-brand mx-auto ms-md-0'} href={'/'}>WannaEat</a>
                </nav>
            </div>
        </div>
    );
};

export default NavBar;