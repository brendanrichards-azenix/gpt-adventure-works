import * as React from 'react'


type LayoutProps = {
    children?: React.ReactNode
}

function Layout(props: LayoutProps) {
    return (
      <div className="container">
        {props.children}
      </div>
    );
}

export default Layout;
  

  