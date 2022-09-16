/*
* https://loading.io/css/
*/
import React, {FC} from 'react';
import './Loader.tsx.css'
import {LoaderProps} from "./LoaderProps";

const Loader: FC<LoaderProps> = ({color}: LoaderProps) => {

    const resultColor = color ?? 'gray';

    return (
        <div>
            <div className={`lds-roller ${resultColor}`}>
                <div/>
                <div/>
                <div/>
                <div/>
                <div/>
                <div/>
                <div/>
                <div/>
            </div>
        </div>
    );
};

export default Loader;
